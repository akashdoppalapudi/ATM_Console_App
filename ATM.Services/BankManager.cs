using ATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class BankManager
    {
        private Bank bank;
        private TransactionHandler transactionHandler;
        private EncryptionService encryptionService;
        private DataHandler dataHandler;

        public BankManager()
        {
            transactionHandler = new TransactionHandler();
            encryptionService = new EncryptionService();
            dataHandler = new DataHandler();
            this.bank = dataHandler.ReadBankData();
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
                    Pin = encryptionService.ComputeSha256Hash(pin),
                    Balance = 1500,
                    Transactions = new List<Transaction>()
                };
                newAccount.Transactions.Add(transactionHandler.NewTransaction(1500, (TransactionType)1));
                this.bank.Accounts.Add(newAccount);
                dataHandler.WriteBankData(this.bank);
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
                account.Transactions.Add(transactionHandler.NewTransaction(amount, (TransactionType)3));
                dataHandler.WriteBankData(this.bank);
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
                account.Transactions.Add(transactionHandler.NewTransaction(amount, (TransactionType)2));
                dataHandler.WriteBankData(this.bank);
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
                    account.Transactions.Add(transactionHandler.NewTransaction(amount, (TransactionType)2));
                    transferToAccount.Balance += amount;
                    transferToAccount.Transactions.Add(transactionHandler.NewTransaction(amount, (TransactionType)3));
                    dataHandler.WriteBankData(this.bank);
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
            string hashedUserInput = encryptionService.ComputeSha256Hash(userInput);
            if (hashedUserInput != account.Pin)
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
