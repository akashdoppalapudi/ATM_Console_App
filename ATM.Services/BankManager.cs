using ATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class BankManager
    {
        private Bank bank;

        public BankManager()
        {
            this.bank = DataHandler.ReadBankData();
            if (this.bank == null)
            {
                this.bank = new Bank
                {
                    Name = "Alpha Bank",
                    Address = "hyderbad",
                    Accounts = new List<Account>()
                };
            }
        }
        public void CreateNewAccount(string name, string pin, AccountType accountType)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(pin) || accountType == (AccountType)0)
            {
                throw new AccountCreationFailedException();
            }
            else
            {
                Account newAccount = new Account
                {
                    AccountId = this.bank.Accounts.Count + 1,
                    AccountHoldersName = name,
                    AccountType = accountType,
                    Pin = EncryptionService.ComputeSha256Hash(pin),
                    Balance = 1500,
                    Transactions = new List<Transaction>()
                };
                newAccount.Transactions.Add(TransactionHandler.NewTransaction(1500, (TransactionType)1));
                this.bank.Accounts.Add(newAccount);
                DataHandler.WriteBankData(this.bank);
            }
        }

        public List<string> GetAllAccounts()
        {
            List<string> accountNames = new List<string>();
            foreach (Account account in this.bank.Accounts)
            {
                accountNames.Add(account.AccountHoldersName);
            }
            return accountNames;
        }

        public void Deposit(int accountId, decimal amount)
        {
            Account account = this.bank.Accounts.Find(a => a.AccountId == accountId);
            if (amount <= 0)
            {
                throw new InvalidAmountException();
            }
            else
            {
                account.Balance += amount;
                account.Transactions.Add(TransactionHandler.NewTransaction(amount, (TransactionType)3));
                DataHandler.WriteBankData(this.bank);
            }
        }


        public void Withdraw(int accountId, decimal amount)
        {
            Account account = this.bank.Accounts.Find(a => a.AccountId == accountId);
            if (amount <= 0 || amount > account.Balance)
            {
                throw new InvalidAmountException();
            }
            else
            {
                account.Balance -= amount;
                account.Transactions.Add(TransactionHandler.NewTransaction(amount, (TransactionType)2));
                DataHandler.WriteBankData(this.bank);
            }
        }


        public void Transfer(int selectedAccountId, int transferToAccountId, decimal amount)
        {
            Account account = this.bank.Accounts.Find(a => a.AccountId == selectedAccountId);
            Account transferToAccount = this.bank.Accounts.Find(a => a.AccountId == transferToAccountId);
            if (amount <= 0 || amount > account.Balance)
            {
                throw new InvalidAmountException();
                throw new TransferFailedException();
            }
            else
            {
                if (transferToAccount == null)
                {
                    throw new TransferFailedException();
                }
                else
                {
                    account.Balance -= amount;
                    account.Transactions.Add(TransactionHandler.NewTransaction(amount, (TransactionType)2));
                    transferToAccount.Balance += amount;
                    transferToAccount.Transactions.Add(TransactionHandler.NewTransaction(amount, (TransactionType)3));
                    DataHandler.WriteBankData(this.bank);
                }
            }

        }

        public List<Transaction> GetTransactions(int accountId)
        {
            Account account = this.bank.Accounts.Find(a => a.AccountId == accountId);
            return account.Transactions;
        }

        public decimal GetBalance(int accountId)
        {
            Account account = this.bank.Accounts.Find(a => a.AccountId == accountId);
            return account.Balance;
        }

        public void CheckAccountExistance(int accountId)
        {
            Account account = this.bank.Accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account == null)
            {
                throw new UserNotFoundException();
            }
        }

        public void Authenticate(int accountId, string userInput)
        {
            Account account = this.bank.Accounts.Find(a => a.AccountId == accountId);
            string hashedUserInput = EncryptionService.ComputeSha256Hash(userInput);
            if (hashedUserInput != account.Pin)
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
