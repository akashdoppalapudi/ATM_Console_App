using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using ATM.Services.IServices;
using AutoMapper;
using System;
using System.Linq;

namespace ATM.Services
{
    public class AccountService : IAccountService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;
        private readonly BankContext _bankContext;

        public AccountService(IEncryptionService encryptionService, BankContext bankContext, IMapper mapper)
        {
            _encryptionService = encryptionService;
            _mapper = mapper;
            _bankContext = bankContext;
        }

        // method name should be changed, unnecessary exception throwing
        public void CheckAccountExistance(string bankId, string accountId)
        {
            if (!_bankContext.Account.Any(a => a.BankId == bankId && a.Id == accountId && a.IsActive))
            {
                throw new AccountDoesNotExistException();
            }
        }

        private Account GetAccountById(string bankId, string accountId)
        {
            // unnecessary check here
            CheckAccountExistance(bankId, accountId);
            return _mapper.Map<Account>(_bankContext.Account.FirstOrDefault(a => a.BankId == bankId && a.Id == accountId && a.IsActive));
        }

        // this is not the right place to keep this method
        public Account CreateAccount(string name, Gender gender, string username, string password, AccountType accountType)
        {
            (byte[] passwordBytes, byte[] saltBytes) = _encryptionService.ComputeHash(password);
            return new Account
            {
                Id = name.GenId(),
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
            // map this bankid before this place
            account.BankId = bankId;
            AccountDBModel accountRecord = _mapper.Map<AccountDBModel>(account);
            _bankContext.Account.Add(accountRecord);
            _bankContext.SaveChanges();
        }

        public string GetAccountIdByUsername(string bankId, string username)
        {
            //unnecessary declaration return it directly
            string id;
            AccountDBModel accountRecord = _bankContext.Account.FirstOrDefault(a => a.BankId == bankId && a.IsActive && a.Username == username);
            if (accountRecord == null)
            {
                throw new AccountDoesNotExistException();
            }
            //id = accountRecord.Id;
            //return id;

            return accountRecord.Id;
        }

        public void UpdateAccount(string bankId, string accountId, Account updateAccount)
        {
            Account account = GetAccountById(bankId, accountId);
            account.Name = updateAccount.Name;
            account.Gender = updateAccount.Gender;
            account.Username = updateAccount.Username;
            if (updateAccount.Password != _encryptionService.ComputeHash("", updateAccount.Salt))
            {
                account.Password = updateAccount.Password;
                account.Salt = updateAccount.Salt;
            }
            account.AccountType = updateAccount.AccountType;
            account.Balance = updateAccount.Balance;
            // all the mappings should be done automapper
            // this is unnecessary fetching from db you are already getting above GetAccountById
            AccountDBModel currentAccountRecord = _bankContext.Account.First(a => a.BankId == account.BankId && a.Id == account.Id && a.IsActive);

            currentAccountRecord.Name = account.Name;
            currentAccountRecord.Gender = account.Gender;
            currentAccountRecord.Username = account.Username;
            currentAccountRecord.Password = account.Password;
            currentAccountRecord.Salt = account.Salt;
            currentAccountRecord.AccountType = account.AccountType;
            currentAccountRecord.Balance = account.Balance;
            _bankContext.SaveChanges();
        }

        public void DeleteAccount(string bankId, string accountId)
        {
            // unnessary check
            CheckAccountExistance(bankId, accountId);
            AccountDBModel accountRecord = _bankContext.Account.First(a => a.Id == accountId && a.BankId == bankId && a.IsActive);
            accountRecord.IsActive = false;
            accountRecord.DeletedOn = DateTime.Now;
            _bankContext.SaveChanges();
        }

        public void Deposit(string bankId, string accountId, Currency currency, decimal amount)
        {
            // no need of getting the account you have account id so directly perform the operation
            Account account = GetAccountById(bankId, accountId);
            if (amount <= 0)
            {
                throw new InvalidAmountException();
            }
            amount *= (decimal)currency.ExchangeRate;
            account.Balance += amount;
            // better update only balance instead of updating whole account
            UpdateAccount(bankId, accountId, account);
        }

        public void Withdraw(string bankId, string accountId, decimal amount)
        {
            Account account = GetAccountById(bankId, accountId);
            if (amount <= 0 || amount > account.Balance)
            {
                throw new InvalidAmountException();
            }
            account.Balance -= amount;
            // same as above
            UpdateAccount(bankId, accountId, account);
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
            // you have already performed this above again you are fetching it

            // think of using deposit and withdraw methods?
            selectedAccount = GetAccountById(selectedBankId, selectedAccountId);
            selectedAccount.Balance -= amount;
            UpdateAccount(selectedBankId, selectedAccountId, selectedAccount);
            Account transferToAccount = GetAccountById(transferToBankId, transferToAccountId);
            transferToAccount.Balance += amount;
            UpdateAccount(transferToBankId, transferToAccountId, transferToAccount);
        }

        public Account GetAccountDetails(string bankId, string accountId)
        {
            Account account = GetAccountById(bankId, accountId);
            // why are you mapping again? even if it is needed you should use auto mapper
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
            // use exists instead of any
            if (_bankContext.Employee.Any(e => e.BankId == bankId && e.Username == username && e.IsActive))
            {
                throw new UsernameAlreadyExistsException();
            }
        }

        public void Authenticate(string bankId, string accountId, string password)
        {
            // cant we compare in another way may be by using where clause
            Account account = GetAccountById(bankId, accountId);
            if (Convert.ToBase64String(account.Password) != Convert.ToBase64String(_encryptionService.ComputeHash(password, account.Salt)))
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
