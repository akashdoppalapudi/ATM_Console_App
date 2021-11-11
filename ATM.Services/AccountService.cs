using ATM.Models;
using ATM.Models.Enums;
using System;
using System.Collections.Generic;
using ATM.Services.Exceptions;
using System.Linq;

namespace ATM.Services
{
    public class AccountService
    {
        private IList<Account> accounts;
        private readonly IDGenService idGenService;
        private readonly EncryptionService encryptionService;
        private readonly DataService dataService;
        private readonly BankService bankService;

        public AccountService()
        {
            idGenService = new IDGenService();
            encryptionService = new EncryptionService();
            dataService = new DataService();
            bankService = new BankService();
            PopulateAccountData();
        }

        private void PopulateAccountData()
        {
            this.accounts = dataService.ReadAccountData();
            if (accounts == null)
            {
                this.accounts = new List<Account>();
            }
        }

        private Account GetAccountById(string bankId, string accountId)
        {
            CheckAccountExistance(bankId, accountId);
            return this.accounts.FirstOrDefault(a => a.Id == accountId && a.BankId == bankId && a.IsActive);
        }

        public Account CreateAccount(string name, Gender gender, string username, string password, AccountType accountType)
        {
            return new Account
            {
                Id = idGenService.GenId(name),
                Name = name,
                Gender = gender,
                Username = username,
                Password = encryptionService.ComputeSha256Hash(password),
                AccountType = accountType
            };
        }

        public void AddAccount(string bankId, Account account)
        {
            PopulateAccountData();
            account.BankId = bankId;
            this.accounts.Add(account);
            dataService.WriteAccountData(this.accounts);
        }

        public void CheckAccountExistance(string bankId, string accountId)
        {
            try
            {
                bankService.CheckBankExistance(bankId);
                PopulateAccountData();
                if(this.accounts.Any(a => a.Id==accountId && a.BankId==bankId && a.IsActive)){
                    return;
                }
                throw new AccountDoesNotExistException();
            }
            catch (BankDoesnotExistException)
            {
                throw new AccountDoesNotExistException();
            }
        }

        public string GetAccountIdByUsername(string bankId, string username)
        {
            Account account = this.accounts.FirstOrDefault(a => a.Username == username && a.BankId==bankId && a.IsActive);
            if (account == null)
            {
                throw new AccountDoesNotExistException();
            }
            return account.Id;
        }

        public void UpdateAccount(string bankId, string accountId, Account updateAccount)
        {
            PopulateAccountData();
            Account account = GetAccountById(bankId, accountId);
            account.Name = updateAccount.Name;
            account.Gender = updateAccount.Gender;
            account.Username = updateAccount.Username;
            if (updateAccount.Password != encryptionService.ComputeSha256Hash(""))
            {
                account.Password = updateAccount.Password;
            }
            account.AccountType = updateAccount.AccountType;
            dataService.WriteAccountData(this.accounts);
        }

        public void DeleteAccount(string bankId, string accountId)
        {
            PopulateAccountData();
            Account account = GetAccountById(bankId, accountId);
            account.IsActive = false;
            account.DeletedOn = DateTime.Now;
            dataService.WriteAccountData(this.accounts);
        }

        public void Deposit(string bankId, string accountId, Currency currency, decimal amount)
        {
            PopulateAccountData();
            Account account = GetAccountById(bankId, accountId);
            if (amount <= 0)
            {
                throw new InvalidAmountException();
            }
            amount *= (decimal)currency.ExchangeRate;
            account.Balance += amount;
            dataService.WriteAccountData(this.accounts);
        }

        public void Withdraw(string bankId, string accountId, decimal amount)
        {
            PopulateAccountData();
            Account account = GetAccountById(bankId, accountId);
            if (amount <= 0 || amount > account.Balance)
            {
                throw new InvalidAmountException();
            }
            account.Balance -= amount;
            dataService.WriteAccountData(this.accounts);
        }

        public void Transfer(string selectedBankId, string selectedAccountId, string transferToBankId, string transferToAccountId, decimal amount)
        {
            PopulateAccountData();
            Account selectedAccount = GetAccountById(selectedBankId, selectedAccountId);
            Account transferToAccount = GetAccountById(transferToBankId, transferToAccountId);
            if (selectedAccountId == transferToAccountId && selectedBankId == transferToBankId)
            {
                throw new AccessDeniedException();
            }
            if (amount <= 0 || amount > selectedAccount.Balance)
            {
                throw new InvalidAmountException();
            }
            selectedAccount.Balance -= amount;
            transferToAccount.Balance += amount;
            dataService.WriteAccountData(this.accounts);
        }

        public Account GetAccountDetails(string bankId, string accountId)
        {
            Account account = GetAccountById(bankId, accountId);
            return new Account
            {
                Name = account.Name,
                Gender = account.Gender,
                Username = account.Username,
                AccountType = account.AccountType
            };
        }

        public decimal GetBalance(string bankId, string accountId)
        {
            PopulateAccountData();
            Account account = GetAccountById(bankId, accountId);
            return account.Balance;
        }

        public void ValidateUsername(string bankId, string username)
        {
            PopulateAccountData();
            if (this.accounts.Any(a => a.BankId == bankId && a.Username == username && a.IsActive))
            {
                throw new UsernameAlreadyExistsException();
            }
        }

        public void Authenticate(string bankId, string accountId, string password)
        {
            PopulateAccountData();
            Account account = GetAccountById(bankId, accountId);
            if (account.Password != encryptionService.ComputeSha256Hash(password))
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
