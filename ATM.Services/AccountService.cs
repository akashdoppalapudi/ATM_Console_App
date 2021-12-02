using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.Exceptions;
using System;

namespace ATM.Services
{
    public class AccountService
    {
        private readonly IDGenService idGenService;
        private readonly EncryptionService encryptionService;
        private readonly DBService dbService;

        public AccountService()
        {
            idGenService = new IDGenService();
            encryptionService = new EncryptionService();
            dbService = new DBService();
        }

        public void CheckAccountExistance(string bankId, string accountId)
        {
            dbService.CheckAccountExistance(bankId, accountId);
        }

        private Account GetAccountById(string bankId, string accountId)
        {
            return dbService.GetAccountById(bankId, accountId);
        }

        public Account CreateAccount(string name, Gender gender, string username, string password, AccountType accountType)
        {
            (byte[] passwordBytes, byte[] saltBytes) = encryptionService.ComputeHash(password);
            return new Account
            {
                Id = idGenService.GenId(name),
                Name = name,
                Gender = gender,
                Username = username,
                Password = passwordBytes,
                Salt = saltBytes,
                AccountType = accountType
            };
        }

        public void AddAccount(string bankId, Account account)
        {
            account.BankId = bankId;
            dbService.AddAccount(account);
        }

        public string GetAccountIdByUsername(string bankId, string username)
        {
            return dbService.GetAccountIdByUsername(bankId, username);
        }

        public void UpdateAccount(string bankId, string accountId, Account updateAccount)
        {
            Account account = GetAccountById(bankId, accountId);
            account.Name = updateAccount.Name;
            account.Gender = updateAccount.Gender;
            account.Username = updateAccount.Username;
            if (updateAccount.Password != encryptionService.ComputeHash("", updateAccount.Salt))
            {
                account.Password = updateAccount.Password;
                account.Salt = updateAccount.Salt;
            }
            account.AccountType = updateAccount.AccountType;
            dbService.UpdateAccount(account);
        }

        public void DeleteAccount(string bankId, string accountId)
        {
            CheckAccountExistance(bankId, accountId);
            dbService.DeleteAccount(bankId, accountId);
        }

        public void Deposit(string bankId, string accountId, Currency currency, decimal amount)
        {
            Account account = GetAccountById(bankId, accountId);
            if (amount <= 0)
            {
                throw new InvalidAmountException();
            }
            amount *= (decimal)currency.ExchangeRate;
            account.Balance += amount;
            dbService.UpdateAccount(account);
        }

        public void Withdraw(string bankId, string accountId, decimal amount)
        {
            Account account = GetAccountById(bankId, accountId);
            if (amount <= 0 || amount > account.Balance)
            {
                throw new InvalidAmountException();
            }
            account.Balance -= amount;
            dbService.UpdateAccount(account);
        }

        public void Transfer(string selectedBankId, string selectedAccountId, string transferToBankId, string transferToAccountId, decimal amount)
        {
            Account selectedAccount = GetAccountById(selectedBankId, selectedAccountId);
            if (selectedAccountId == transferToAccountId && selectedBankId == transferToBankId)
            {
                throw new AccessDeniedException();
            }
            if (amount <= 0 || amount > selectedAccount.Balance)
            {
                throw new InvalidAmountException();
            }
            selectedAccount = GetAccountById(selectedBankId, selectedAccountId);
            selectedAccount.Balance -= amount;
            dbService.UpdateAccount(selectedAccount);
            Account transferToAccount = GetAccountById(transferToBankId, transferToAccountId);
            transferToAccount.Balance += amount;
            dbService.UpdateAccount(transferToAccount);
        }

        public Account GetAccountDetails(string bankId, string accountId)
        {
            Account account = GetAccountById(bankId, accountId);
            return new Account
            {
                BankId = account.BankId,
                Name = account.Name,
                Gender = account.Gender,
                Username = account.Username,
                AccountType = account.AccountType
            };
        }

        public decimal GetBalance(string bankId, string accountId)
        {
            Account account = GetAccountById(bankId, accountId);
            return account.Balance;
        }

        public void ValidateUsername(string bankId, string username)
        {
            dbService.ValidateAccountUsername(bankId, username);
        }

        public void Authenticate(string bankId, string accountId, string password)
        {
            Account account = GetAccountById(bankId, accountId);
            if (Convert.ToBase64String(account.Password) != Convert.ToBase64String(encryptionService.ComputeHash(password, account.Salt)))
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
