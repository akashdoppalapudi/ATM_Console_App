using ATM.Models;
using ATM.Models.Enums;
using System;

namespace ATM.Services
{
    public class AccountService
    {
        IDGenService idGenService;
        EncryptionService encryptionService;
        public AccountService()
        {
            idGenService = new IDGenService();
            encryptionService = new EncryptionService();
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

        public void AddTransaction(Account account, Transaction transaction)
        {
            account.Transactions.Add(transaction);
        }

        public void UpdateAccount(Account account, Account updateAccount)
        {
            account.Name = updateAccount.Name;
            account.Gender = updateAccount.Gender;
            account.Username = updateAccount.Username;
            if (updateAccount.Password != encryptionService.ComputeSha256Hash(""))
            {
                account.Password = updateAccount.Password;
            }
            account.AccountType = updateAccount.AccountType;
            account.UpdatedOn = DateTime.Now;
        }

        public void DeleteAccount(Account account)
        {
            account.IsActive = false;
            account.UpdatedOn = DateTime.Now;
            account.DeletedOn = DateTime.Now;
        }

        public void Deposit(Account account, decimal amount, Currency currency)
        {
            account.Balance += amount * (decimal)currency.ExchangeRate;
        }

        public void Withdraw(Account account, decimal amount)
        {
            account.Balance -= amount;
        }

        public void Transfer(Account fromAccount, Account toAccount, decimal amount)
        {
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;
        }

        public bool Authenticate(Account account, string password)
        {
            return account.Password == encryptionService.ComputeSha256Hash(password);
        }
    }
}
