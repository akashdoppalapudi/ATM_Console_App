using ATM.Models;
using System;

namespace ATM.Services
{
    public class ConsoleUI
    {
        public static (string, int, AccountType) getDataForAccountCreation()
        {
            string name;
            int pin;
            AccountType accountType;
            Console.WriteLine("____ACCOUNT CREATION____\n");
            Console.Write("Please Enter your name : ");
            string selectedName = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedName))
            {
                Console.WriteLine("Invalid Name");
                return (null, -1, (AccountType)0);
            }
            name = selectedName;

            Console.Write("Please set a 4-digit PIN : ");
            string selectedPin = Console.ReadLine();

            if (selectedPin.Length != 4)
            {
                Console.WriteLine("Invalid PIN");
                return (null, -1, (AccountType)0);
            }

            try
            {
                pin = Convert.ToInt32(selectedPin);
            }
            catch
            {
                Console.WriteLine("Invalid PIN");
                return (null, -1, (AccountType)0);
            }

            Console.WriteLine("__ACCOUNT TYPE__\n");
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
                if((int)accountType <= 0 || (int)accountType >= i){
                    Console.WriteLine("Invalid Account Type");
                    return (null, -1, (AccountType)0);
                }
            }
            catch
            {
                Console.WriteLine("Invalid Account Type");
                return (null, -1, (AccountType)0);
            }

            return (name, pin, accountType);
            
        }
    }
}
