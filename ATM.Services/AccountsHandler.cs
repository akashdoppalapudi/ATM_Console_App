using ATM.Models;
using System;
using System.Collections.Generic;

namespace ATM.Services
{
    public class AccountsHandler
    {
        static Bank bank;

        public AccountsHandler()
        {
            if (bank == null)
            {
                bank = new Bank
                {
                    accounts = new List<Account>()
                };
            }
        }
        public void createNewAccount()
        {
            (string name, int pin, AccountType accountType) = ConsoleUI.getDataForAccountCreation();
            if (String.IsNullOrEmpty(name) || pin == -1 || accountType == (AccountType)0)
            {
                StandardMessages.accountCreationFailed();
            }
            Account newAccount = new Account
            {
                accountNumber = bank.accounts.Count + 1,
                accountHoldersName = name,
                accountType = accountType,
                pin = pin,
                availableBalance = 1500,
                transactions = new List<Transaction>()
            };
            newAccount.transactions.Add(TransactionHandler.accountCreationTransaction());
            bank.accounts.Add(newAccount);
            StandardMessages.accountCreationSuccess();
        }
    }
}
