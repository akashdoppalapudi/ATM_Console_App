using ATM.CLI;
using ATM.Models;
using ATM.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class ConsoleUI
    {
        ConsoleMessages consoleMessages = new ConsoleMessages();
        public Tuple<string, Gender, string, string, AccountType> GetDataForAccountCreation()
        {
            string name, username, password;
            Gender gender;
            AccountType accountType;
            Console.WriteLine("\n____ACCOUNT CREATION____\n");
            Console.Write("Please Enter Name : ");
            string selectedName = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedName))
            {
                Console.WriteLine("Invalid Name");
                return null;
            }
            name = selectedName;

            Console.WriteLine("\n__GENDER__\n");
            int i = 1;
            foreach (string g in Enum.GetNames(typeof(Gender)))
            {
                Console.WriteLine(i + ". " + g);
                i++;
            }
            Console.Write("\nSelect a Gender : ");
            string selectedGender = Console.ReadLine();
            try
            {
                gender = (Gender)Convert.ToInt32(selectedGender);
                if ((int)gender <= 0 || (int)gender >= i)
                {
                    Console.WriteLine("Invalid Gender");
                    return null;
                }
            }
            catch
            {
                Console.WriteLine("Invalid Gender");
                return null;
            }

            Console.Write("\nPlease set a Username : ");
            string selectedUsername = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedUsername))
            {
                return null;
            }
            username = selectedUsername;

            Console.Write("Please set a Password : ");
            string selectedPassword = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedPassword))
            {
                Console.WriteLine("Invalid Password");
                return null;
            }
            password = selectedPassword;

            Console.WriteLine("\n__ACCOUNT TYPE__\n");
            i = 1;
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
                    return null;
                }
            }
            catch
            {
                Console.WriteLine("Invalid Account Type");
                return null;
            }

            return Tuple.Create(name, gender, selectedUsername, password, accountType);
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
                    return bankNames.ElementAt(selectedOption - 1).Key;
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
                Console.WriteLine(transaction.Id + "\t" + transaction.TransactionDate + "\t" + transaction.TransactionType + "\tRs. " + transaction.TransactionAmount);
            }
            Console.WriteLine("\t\t\t\tAvailable Balance : " + availBal);
        }

        public Option1 SelectOrCreateBank()
        {
            Option1 option;
            Console.WriteLine("\n____OPTIONS____\n");
            Console.WriteLine("1. Create New Bank\n2. Select a Bank\n3. Exit");
            Console.Write("\nSelect an Option : ");
            string userInput = Console.ReadLine();
            try
            {
                option = (Option1)Convert.ToInt32(userInput);
            }
            catch
            {
                option = (Option1)0;
            }
            return option;
        }

        public Option1 ExistingOrCreate()
        {
            Option1 option;
            Console.WriteLine("\n____OPTIONS____\n");
            Console.WriteLine("1. Create New Account\n2. Existing User");
            Console.Write("\nSelect an Option : ");
            string userInput = Console.ReadLine();
            try
            {
                option = (Option1)(Convert.ToInt32(userInput) + 3);
            }
            catch
            {
                option = (Option1)0;
            }
            return option;
        }

        public Option1 SelectOperation()
        {
            Option1 operation;
            Console.WriteLine("\n____OPERATIONS____\n");
            Console.WriteLine("1. Deposit\n2. Withdraw\n3. Transfer\n4. Show Transaction History");
            Console.Write("\nSelect an operation : ");
            string userInput = Console.ReadLine();
            try
            {
                operation = (Option1)(Convert.ToInt32(userInput) + 5);
            }
            catch
            {
                operation = (Option1)0;
            }
            return operation;
        }
    }
}
