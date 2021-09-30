using System;

namespace ATM.Services
{
    public class StandardMessages
    {
        public static void accountCreationFailed()
        {
            Console.WriteLine("Account Creation Failed");
        }

        public static void accountCreationSuccess()
        {
            Console.WriteLine("Account Created Successfully");
        }

        public static void availableBalanceMsg(decimal availBal)
        {
            Console.WriteLine("\t\tAvailable Balance : " + availBal);
        }

        public static void welcomeMsg()
        {
            Console.WriteLine("Welcome to ALPHA BANK");
        }

        public static void invalidOptionMsg()
        {
            Console.WriteLine("Invalid Option");
        }

        public static void invalidAmountMsg()
        {
            Console.WriteLine("Invalid Amount");
        }

        public static void wrongPinMsg()
        {
            Console.WriteLine("Wrong PIN");
        }

        public static void transferSuccess()
        {
            Console.WriteLine("Amount Transfered Successfully");
        }

        public static void transferFailed()
        {
            Console.WriteLine("Transfer Failed");
        }

        public static void depositSuccess()
        {
            Console.WriteLine("Amount Deposited Successfully");
        }

        public static void withdrawSuccess()
        {
            Console.WriteLine("Amount Withdrawn Successfully");
        }
    }
}
