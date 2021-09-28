using System;
using System.Collections.Generic;

namespace ATM
{
    public class ConsoleUI
    {
        public static string getPinFromUser()
        {
            Console.WriteLine("\n____AUTHENTICATION____");
            Console.Write("Enter PIN : ");
            string userInput = Console.ReadLine();
            return userInput;
        }

        public static string welcome()
        {
            Console.WriteLine("\n\n____ATM____");
            Console.WriteLine("1. Create New Account\n2. Exixting User");
            Console.Write("Select an Option (Enter 'e' to exit) : ");
            string option = Console.ReadLine();
            return option;
        }

        public static string selectAccount(List<Account> accounts)
        {
            Console.WriteLine("\n\n____EXISTING ACCOUNTS____");
            foreach (Account acc in accounts)
            {
                Console.WriteLine(acc.AccountNumber + ". " + acc.Name);
            }
            Console.Write("Select your Account (Enter 'b' to go back) : ");
            string selectedAcc = Console.ReadLine();
            return selectedAcc;
        }

        public static string selectOperation()
        {
            Console.WriteLine("\n\n____OPERATIONS____");
            Console.WriteLine("\n1. Deposit\n2. Withdraw\n3. Transfer\n4. Show Transaction History");
            Console.Write("Select an operation (Enter 'b' to go back) : ");
            string operation = Console.ReadLine();
            return operation;
        }

        public static void wrongPinMsg()
        {
            Console.WriteLine("Wrong Pin!");
        }

        public static void invalidAmountMsg()
        {
            Console.WriteLine("Invalid Amount");
        }

        public static void invalidOptionMsg()
        {
            Console.WriteLine("Invalid Option!");
        }

        public static void succesMsg(char msgFor)
        {
            switch (msgFor)
            {
                case 'd':
                    Console.WriteLine("Amount deposited successfully");
                    break;
                case 'w':
                    Console.WriteLine("Amount withdrawn successfully");
                    break;
                case 't':
                    Console.WriteLine("Amount transferred successfully");
                    break;
            }
        }

        public static (string name, int pin) accountCreationInfo()
        {
            int selectedPin = -1;
            Console.WriteLine("\n\n____CREATING ACCOUNT____");
            Console.Write("Enter Your name : ");
            string selectedName = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedName))
            {
                selectedName = "____";
                Console.WriteLine("Invalid Name!");
                return (selectedName, selectedPin);
            }
            Console.Write("Set a 4-digit pin : ");
            string userInput = Console.ReadLine();
            if (userInput.Length != 4)
            {
                selectedPin = -1;
                Console.WriteLine("Invalid Pin!");
                return (selectedName, selectedPin);
            } else
            {
                try
                {
                    selectedPin = int.Parse(userInput);
                } catch
                {
                    selectedPin = -1;
                    Console.WriteLine("Invalid Pin!");
                }
                return (selectedName, selectedPin);
            }
        }

        public static int getAmmount(char amtFor)
        {
            int amount;
            switch (amtFor)
            {
                case 'd':
                    Console.WriteLine("\n\n____DEPOSIT____");
                    Console.Write("Enter Amount to be deposited : ");
                    break;
                case 'w':
                    Console.WriteLine("\n\n____WITHDRAW____");
                    Console.Write("Enter Amount to be withdrawn : ");
                    break;
                case 't':
                    Console.WriteLine("\n\n____TRANSFER____");
                    Console.Write("Enter Amount to be transferred : ");
                    break;
            }
            string userInput = Console.ReadLine();
            try
            {
                amount = int.Parse(userInput);
            } catch
            {
                amount = -1;
            }
            return amount;
        }
    }
}
