using ATM.CLI;
using ATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class ConsoleUI
    {
        ConsoleMessages consoleMessages = new ConsoleMessages();
        public (string, string, string, AccountType) GetDataForAccountCreation()
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
                return (null, null, null, (AccountType)0);
            }
            name = selectedName;

            Console.Write("Please set a 4-digit PIN : ");
            string selectedPin = Console.ReadLine();

            if (selectedPin.Length != 4)
            {
                Console.WriteLine("Invalid PIN");
                return (null, null, null, (AccountType)0);
            }

            try
            {
                pin = Convert.ToInt32(selectedPin);
            }
            catch
            {
                Console.WriteLine("Invalid PIN");
                return (null, null, null, (AccountType)0);
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
                    return (null, null, null, (AccountType)0);
                }
            }
            catch
            {
                Console.WriteLine("Invalid Account Type");
                return (null, null, null, (AccountType)0);
            }

            Console.Write("\nEnter a Username : ");
            string selectedUsername = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedUsername))
            {
                return (null, null, null, (AccountType)0);
            }

            return (name, Convert.ToString(pin), selectedUsername, accountType);
        }

        public string GetBankName()
        {
            Console.WriteLine("\n____Bank Creation____\n");
            Console.Write("Enter Bank Name : ");
            string name = Console.ReadLine();
            if (String.IsNullOrEmpty(name))
            {
                Console.WriteLine("Invalid Bank Name");
                return null;
            }
            return name;
        }

        public string SelectBank(Dictionary<string, string> bankNames)
        {
            Console.WriteLine("\n____BANKS____\n");
            int i = 1;
            foreach (string name in bankNames.Values)
            {
                Console.WriteLine(i + ". " + name);
                i++;
            }
            Console.Write("\nSelect a Bank : ");
            string userInput = Console.ReadLine();
            try
            {
                int selectedOption = Convert.ToInt32(userInput);
                if (selectedOption >= 1 && selectedOption <= bankNames.Count)
                {
                    return bankNames.ElementAt(selectedOption-1).Key;
                }
                else
                {
                    consoleMessages.InvalidOptionMsg();
                    return null;
                }
            }
            catch
            {
                consoleMessages.InvalidOptionMsg();
                return null;
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

        public string GetUsername()
        {
            Console.Write("\nEnter your Username : ");
            string username = Console.ReadLine();
            return username;
        }

        public string GetTransferToUsername()
        {
            Console.Write("\nEnter Reciever's Username : ");
            string username = Console.ReadLine();
            return username;
        }

        public string GetPinFromUser()
        {
            Console.WriteLine("\n____AUTHENTICATION____\n");
            Console.Write("Enter PIN : ");
            string userInput = Console.ReadLine();
            return userInput;
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

        public Option SelectOrCreateBank()
        {
            Option option;
            Console.WriteLine("\n____OPTIONS____\n");
            Console.WriteLine("1. Create New Bank\n2. Select a Bank\n3. Exit");
            Console.Write("\nSelect an Option : ");
            string userInput = Console.ReadLine();
            try
            {
                option = (Option)Convert.ToInt32(userInput);
            }
            catch
            {
                option = (Option)0;
            }
            return option;
        }

        public Option ExistingOrCreate()
        {
            Option option;
            Console.WriteLine("\n____OPTIONS____\n");
            Console.WriteLine("1. Create New Account\n2. Exixting User");
            Console.Write("\nSelect an Option : ");
            string userInput = Console.ReadLine();
            try
            {
                option = (Option)(Convert.ToInt32(userInput) + 3);
            }
            catch
            {
                option = (Option)0;
            }
            return option;
        }

        public Option SelectOperation()
        {
            Option operation;
            Console.WriteLine("\n____OPERATIONS____\n");
            Console.WriteLine("1. Deposit\n2. Withdraw\n3. Transfer\n4. Show Transaction History");
            Console.Write("\nSelect an operation : ");
            string userInput = Console.ReadLine();
            try
            {
                operation = (Option)(Convert.ToInt32(userInput) + 5);
            }
            catch
            {
                operation = (Option)0;
            }
            return operation;
        }
    }
}
