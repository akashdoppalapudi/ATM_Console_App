using ATM.Models;
using System;
using System.Collections.Generic;

namespace ATM.Services
{
    public class ConsoleUI
    {
        ConsoleMessages consoleMessages = new ConsoleMessages();
        public (string, string, AccountType) GetDataForAccountCreation()
        {
            string name;
            int pin;
            AccountType accountType;
            Console.WriteLine("\n____ACCOUNT CREATION____\n");
            Console.Write("Please Enter your name : ");
            string selectedName = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedName))
            {
                Console.WriteLine("Invalid Name");
                return (null, null, (AccountType)0);
            }
            name = selectedName;

            Console.Write("Please set a 4-digit PIN : ");
            string selectedPin = Console.ReadLine();

            if (selectedPin.Length != 4)
            {
                Console.WriteLine("Invalid PIN");
                return (null, null, (AccountType)0);
            }

            try
            {
                pin = Convert.ToInt32(selectedPin);
            }
            catch
            {
                Console.WriteLine("Invalid PIN");
                return (null, null, (AccountType)0);
            }

            Console.WriteLine("\n__ACCOUNT TYPE__\n");
            int i = 1;
            foreach (string type in Enum.GetNames(typeof(AccountType)))
            {
                Console.WriteLine(i + ". " + type);
                i++;
            }
            Console.Write("\nSelect an Account type : ");
            string selectedType = Console.ReadLine();

            try
            {
                accountType = (AccountType)Convert.ToInt32(selectedType);
                if ((int)accountType <= 0 || (int)accountType >= i)
                {
                    Console.WriteLine("Invalid Account Type");
                    return (null, null, (AccountType)0);
                }
            }
            catch
            {
                Console.WriteLine("Invalid Account Type");
                return (null, null, (AccountType)0);
            }

            return (name, Convert.ToString(pin), accountType);

        }

        public int SelectAccount(List<string> accountNames)
        {
            Console.WriteLine("\n____ALL ACCOUNTS____\n");
            int i = 1;
            foreach (string accountName in accountNames)
            {
                Console.WriteLine(i + ". " + accountName);
                i++;
            }
            Console.Write("\nSelect your account : ");
            string choice = Console.ReadLine();
            try
            {
                int selectedAccountId = Convert.ToInt32(choice);
                return selectedAccountId;
            }
            catch
            {
                consoleMessages.InvalidOptionMsg();
                return -1;
            }
        }

        public decimal GetAmount(char amtFor)
        {
            decimal amount;
            switch (amtFor)
            {
                case 'd':
                    Console.WriteLine("\n____DEPOSIT____\n");
                    Console.Write("Enter Amount to be deposited : ");
                    break;
                case 'w':
                    Console.WriteLine("\n____WITHDRAW____\n");
                    Console.Write("Enter Amount to be withdrawn : ");
                    break;
                case 't':
                    Console.WriteLine("\n\n____TRANSFER____\n");
                    Console.Write("Enter Amount to be transferred : ");
                    break;
            }
            string userInput = Console.ReadLine();
            try
            {
                amount = decimal.Parse(userInput);
            }
            catch
            {
                amount = -1;
            }
            return amount;
        }

        public string GetPinFromUser()
        {
            Console.WriteLine("\n____AUTHENTICATION____\n");
            Console.Write("Enter PIN : ");
            string userInput = Console.ReadLine();
            return userInput;
        }

        public int SelectTransferToAccountId(int fromAccId, List<string> accountNames)
        {
            int selectedAccountId;
            Console.WriteLine();
            int i = 1;
            foreach (string accountName in accountNames)
            {
                if (i != fromAccId)
                {
                    Console.WriteLine(i + ". " + accountName);
                }
                i++;
            }
            Console.Write("Select an Account to transfer money to : ");
            string userInput = Console.ReadLine();
            try
            {
                selectedAccountId = int.Parse(userInput);
                if (selectedAccountId > 0 && selectedAccountId <= i && selectedAccountId != fromAccId)
                {
                    return selectedAccountId;
                }
                else
                {
                    consoleMessages.InvalidOptionMsg();
                    return -1;
                }
            }
            catch
            {
                consoleMessages.InvalidOptionMsg();
                return -1;
            }
        }

        public void PrintTransactions(List<Transaction> transactions, decimal availBal)
        {
            Console.WriteLine("\n____TRANSACTION HISTORY____\n");
            foreach (Transaction transaction in transactions)
            {
                Console.WriteLine(transaction.TransactionDate + "\t" + transaction.TransactionType + "\tRs. " + transaction.TransactionAmount);
            }
            Console.WriteLine("\t\tAvailable Balance : " + availBal);
        }

        public string ExistingOrCreate()
        {
            Console.WriteLine("\n____ATM____\n");
            Console.WriteLine("1. Create New Account\n2. Exixting User\n3. Exit");
            Console.Write("\nSelect an Option : ");
            string option = Console.ReadLine();
            return option;
        }

        public string SelectOperation()
        {
            Console.WriteLine("\n____OPERATIONS____\n");
            Console.WriteLine("1. Deposit\n2. Withdraw\n3. Transfer\n4. Show Transaction History");
            Console.Write("\nSelect an operation : ");
            string operation = Console.ReadLine();
            return operation;
        }
    }
}
