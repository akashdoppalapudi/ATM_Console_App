using ATM.Models;
using System;
using System.Collections.Generic;

namespace ATM.Services
{
    public class AccountsHandler
    {
        public static Bank bank;

        public AccountsHandler()
        {
            bank = DataHandler.readBankData();
            if (bank == null)
            {
                bank = new Bank
                {
                    accounts = new List<Account>()
                };
            }
        }
        public void createNewAccount(string name, string pin, AccountType accountType)
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
                    pin = Encryption.computeSha256Hash(pin),
                    availableBalance = 1500,
                    transactions = new List<Transaction>()
                };
                newAccount.transactions.Add(TransactionHandler.newTransaction(1500, (TransactionType)1));
                bank.accounts.Add(newAccount);
                DataHandler.writeBankData(bank);
            }
        }

        public List<Account> getAllAccounts()
        {
            return bank.accounts;
        }

        public void deposit(Account account, decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidAmountException();
            }
            else
            {
                account.availableBalance += amount;
                account.transactions.Add(TransactionHandler.newTransaction(amount, (TransactionType)3));
                DataHandler.writeBankData(bank);
            }
        }


        public void withdraw(Account account, decimal amount)
        {
            if (amount <= 0 || amount > account.availableBalance)
            {
                throw new InvalidAmountException();
            }
            else
            {
                account.availableBalance -= amount;
                account.transactions.Add(TransactionHandler.newTransaction(amount, (TransactionType)2));
                DataHandler.writeBankData(bank);
            }
        }


        public void transfer(Account account, Account transferToAccount, decimal amount)
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
                    account.transactions.Add(TransactionHandler.newTransaction(amount, (TransactionType)2));
                    recieve(transferToAccount, amount);
                }
            }

        }

        public List<Transaction> getTransactions(Account account)
        {
            return account.transactions;
        }

        private void recieve(Account account, decimal amount)
        {
            account.availableBalance += amount;
            account.transactions.Add(TransactionHandler.newTransaction(amount, (TransactionType)3));
            DataHandler.writeBankData(bank);
        }

        public bool authenticate(Account account, string userInput)
        {
            string hashedUserInput = Encryption.computeSha256Hash(userInput);
            if (hashedUserInput == account.pin)
            {
                return true;
            }
            return false;
        }
    }
}
