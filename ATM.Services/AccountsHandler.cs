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
                StandardMessages.accountCreationFailed();
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
                StandardMessages.accountCreationSuccess();
            }
        }

        public List<Account> getAllAccounts()
        {
            return bank.accounts;
        }

        public void deposit(Account account, decimal amount, string userInputPin)
        {
            if (authenticate(account, userInputPin))
            {
                if (amount <= 0)
                {
                    StandardMessages.invalidAmountMsg();
                }
                else
                {
                    account.availableBalance += amount;
                    account.transactions.Add(TransactionHandler.newTransaction(amount, (TransactionType)3));
                    DataHandler.writeBankData(bank);
                    StandardMessages.depositSuccess();
                }
            }
        }

        public void withdraw(Account account, decimal amount, string userInputPin)
        {
            if (authenticate(account, userInputPin))
            {
                if (amount <= 0 || amount > account.availableBalance)
                {
                    StandardMessages.invalidAmountMsg();
                }
                else
                {
                    account.availableBalance -= amount;
                    account.transactions.Add(TransactionHandler.newTransaction(amount, (TransactionType)2));
                    DataHandler.writeBankData(bank);
                    StandardMessages.withdrawSuccess();
                }
            }
        }

        public void transfer(Account account, Account transferToAccount, decimal amount, string userInputPin)
        {
            if (authenticate(account, userInputPin))
            {
                if (amount <= 0 || amount > account.availableBalance)
                {
                    StandardMessages.invalidAmountMsg();
                    StandardMessages.transferFailed();
                }
                else
                {
                    if (transferToAccount == null)
                    {
                        StandardMessages.transferFailed();
                    }
                    else
                    {
                        account.availableBalance -= amount;
                        account.transactions.Add(TransactionHandler.newTransaction(amount, (TransactionType)2));
                        recieve(transferToAccount, amount);
                    }
                }
            }
        }

        public List<Transaction> getTransactions(Account account, string userInputPin)
        {
            if (authenticate(account, userInputPin))
            {
                return account.transactions;
            }
            return null;
        }

        private void recieve(Account account, decimal amount)
        {
            account.availableBalance += amount;
            account.transactions.Add(TransactionHandler.newTransaction(amount, (TransactionType)3));
            DataHandler.writeBankData(bank);
            StandardMessages.transferSuccess();
        }

        private static bool authenticate(Account account, string userInput)
        {
            string hashedUserInput = Encryption.computeSha256Hash(userInput);
            if (hashedUserInput == account.pin)
            {
                return true;
            }
            StandardMessages.wrongPinMsg();
            return false;
        }
    }
}
