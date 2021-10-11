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
                    accounts = new List<Account>()
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
                    accountNumber = bank.accounts.Count + 1,
                    accountHoldersName = name,
                    accountType = accountType,
                    pin = EncryptionService.ComputeSha256Hash(pin),
                    availableBalance = 1500,
                    transactions = new List<Transaction>()
                };
                newAccount.transactions.Add(TransactionHandler.NewTransaction(1500, (TransactionType)1));
                bank.accounts.Add(newAccount);
                DataHandler.WriteBankData(bank);
            }
        }

        public List<Account> GetAllAccounts()
        {
            return bank.accounts;
        }

        public void Deposit(Account account, decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidAmountException();
            }
            else
            {
                account.availableBalance += amount;
                account.transactions.Add(TransactionHandler.NewTransaction(amount, (TransactionType)3));
                DataHandler.WriteBankData(bank);
            }
        }


        public void Withdraw(Account account, decimal amount)
        {
            if (amount <= 0 || amount > account.availableBalance)
            {
                throw new InvalidAmountException();
            }
            else
            {
                account.availableBalance -= amount;
                account.transactions.Add(TransactionHandler.NewTransaction(amount, (TransactionType)2));
                DataHandler.WriteBankData(bank);
            }
        }


        public void Transfer(Account account, Account transferToAccount, decimal amount)
        {
            if (amount <= 0 || amount > account.availableBalance)
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
                    account.availableBalance -= amount;
                    account.transactions.Add(TransactionHandler.NewTransaction(amount, (TransactionType)2));
                    transferToAccount.availableBalance += amount;
                    transferToAccount.transactions.Add(TransactionHandler.NewTransaction(amount, (TransactionType)3));
                    DataHandler.WriteBankData(bank);
                }
            }

        }

        public List<Transaction> GetTransactions(Account account)
        {
            return account.transactions;
        }

        public void Authenticate(Account account, string userInput)
        {
            string hashedUserInput = EncryptionService.ComputeSha256Hash(userInput);
            if (hashedUserInput != account.pin)
            {
                throw new AuthenticationFailedException();
            }
        }
    }
}
