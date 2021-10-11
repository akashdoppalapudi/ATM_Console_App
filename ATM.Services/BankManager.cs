using ATM.Models;
using System;
using System.Collections.Generic;

namespace ATM.Services
{
    public class BankManager
    {
        public static Bank bank;

        public BankManager()
        {
            bank = DataHandler.ReadBankData();
            if (bank == null)
            {
                bank = new Bank
                {
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
                    AccountId = bank.Accounts.Count + 1,
                    AccountHoldersName = name,
                    AccountType = accountType,
                    Pin = EncryptionService.ComputeSha256Hash(pin),
                    Balance = 1500,
                    Transactions = new List<Transaction>()
                };
                newAccount.Transactions.Add(TransactionHandler.NewTransaction(1500, (TransactionType)1));
                bank.Accounts.Add(newAccount);
                DataHandler.WriteBankData(bank);
            }
        }

        public List<Account> GetAllAccounts()
        {
            return bank.Accounts;
        }

        public void Deposit(Account account, decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidAmountException();
            }
            else
            {
                account.Balance += amount;
                account.Transactions.Add(TransactionHandler.NewTransaction(amount, (TransactionType)3));
                DataHandler.WriteBankData(bank);
            }
        }


        public void Withdraw(Account account, decimal amount)
        {
            if (amount <= 0 || amount > account.Balance)
            {
                throw new InvalidAmountException();
            }
            else
            {
                account.Balance -= amount;
                account.Transactions.Add(TransactionHandler.NewTransaction(amount, (TransactionType)2));
                DataHandler.WriteBankData(bank);
            }
        }


        public void Transfer(Account account, Account transferToAccount, decimal amount)
        {
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
                    DataHandler.WriteBankData(bank);
                }
            }

        }

        public List<Transaction> GetTransactions(Account account)
        {
            return account.Transactions;
        }

        public void Authenticate(Account account, string userInput)
        {
            string hashedUserInput = EncryptionService.ComputeSha256Hash(userInput);
            if (hashedUserInput != account.Pin)
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
