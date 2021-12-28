using ATM.Models;
using ATM.Models.ViewModels;
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

        public void AddAccount(Account account)
        {
            AccountDBModel accountRecord = _mapper.Map<AccountDBModel>(account);
            _bankContext.Account.Add(accountRecord);
            _bankContext.SaveChanges();
        }

        public string GetAccountIdByUsername(string bankId, string username)
        {
            AccountDBModel accountRecord = _bankContext.Account.FirstOrDefault(a => a.BankId == bankId && a.IsActive && a.Username == username);
            if (accountRecord == null)
            {
                throw new AccountDoesNotExistException();
            }

            return accountRecord.Id;
        }

        public void UpdateAccount(string accountId, Account updateAccount)
        {
            // all the mappings should be done automapper - [Updates done through automapper are not working]
            AccountDBModel currentAccountRecord = _bankContext.Account.FirstOrDefault(a => a.Id == accountId && a.IsActive);
            if (currentAccountRecord == null)
            {
                throw new AccountDoesNotExistException();
            }
            currentAccountRecord.Name = updateAccount.Name;
            currentAccountRecord.Gender = updateAccount.Gender;
            currentAccountRecord.Username = updateAccount.Username;
            if (Convert.ToBase64String(_encryptionService.ComputeHash("", updateAccount.Salt)) == Convert.ToBase64String(updateAccount.Password))
            {
                currentAccountRecord.Password = updateAccount.Password;
                currentAccountRecord.Salt = updateAccount.Salt;
            }
            currentAccountRecord.AccountType = updateAccount.AccountType;
            _bankContext.SaveChanges();
        }

        public void DeleteAccount(string accountId)
        {
            AccountDBModel accountRecord = _bankContext.Account.FirstOrDefault(a => a.Id == accountId && a.IsActive);
            if (accountRecord == null)
            {
                throw new AccountDoesNotExistException();
            }
            accountRecord.IsActive = false;
            accountRecord.DeletedOn = DateTime.Now;
            _bankContext.SaveChanges();
        }

        public void Deposit(string accountId, Currency currency, decimal amount)
        {
            AccountDBModel accountRecord = _bankContext.Account.FirstOrDefault(a => a.Id == accountId && a.IsActive);
            if (accountRecord == null)
            {
                throw new AccountDoesNotExistException();
            }
            if (amount <= 0)
            {
                throw new InvalidAmountException();
            }
            amount *= (decimal)currency.ExchangeRate;
            accountRecord.Balance += amount;
            _bankContext.SaveChanges();
        }

        public void Withdraw(string accountId, decimal amount)
        {
            AccountDBModel accountRecord = _bankContext.Account.FirstOrDefault(a => a.Id == accountId && a.IsActive);
            if (accountRecord == null)
            {
                throw new AccountDoesNotExistException();
            }
            if (amount <= 0 || amount > accountRecord.Balance)
            {
                throw new InvalidAmountException();
            }
            accountRecord.Balance -= amount;
            _bankContext.SaveChanges();
        }

        public void Transfer(string selectedAccountId, string transferToAccountId, decimal amount)
        {
            if (selectedAccountId == transferToAccountId)
            {
                throw new AccessDeniedException();
            }
            Withdraw(selectedAccountId, amount);
            Deposit(transferToAccountId, new Currency { Name = "INR", ExchangeRate = 1 }, amount);
        }

        public AccountViewModel GetAccountDetails(string accountId)
        {
            AccountDBModel accountRecord = _bankContext.Account.FirstOrDefault(a => a.Id == accountId && a.IsActive);
            if (accountRecord == null)
            {
                throw new AccountDoesNotExistException();
            }
            return _mapper.Map<AccountViewModel>(accountRecord);
        }

        public decimal GetBalance(string accountId)
        {
            AccountDBModel accountRecord = _bankContext.Account.FirstOrDefault(a => a.Id == accountId && a.IsActive);
            if (accountRecord == null)
            {
                throw new AccountDoesNotExistException();
            }
            return accountRecord.Balance;
        }

        public void ValidateUsername(string bankId, string username)
        {
            // use exists instead of any - [exists wont work on DbSet]
            if (_bankContext.Account.Any(a => a.BankId == bankId && a.Username == username && a.IsActive))
            {
                throw new UsernameAlreadyExistsException();
            }
        }

        public void Authenticate(string accountId, string password)
        {
            // cant we compare in another way may be by using where clause
            AccountDBModel accountRecord = _bankContext.Account.FirstOrDefault(a => a.Id == accountId && a.IsActive);
            if (accountRecord == null)
            {
                throw new AccountDoesNotExistException();
            }
            if (Convert.ToBase64String(accountRecord.Password) != Convert.ToBase64String(_encryptionService.ComputeHash(password, accountRecord.Salt)))
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
