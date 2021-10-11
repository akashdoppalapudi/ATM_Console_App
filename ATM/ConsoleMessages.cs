using System;

namespace ATM.Services
{
    public class ConsoleMessages
    {
        public static void AccountCreationFailed()
        {
            Console.WriteLine("Account Creation Failed");
        }

        public static void AccountCreationSuccess()
        {
            Console.WriteLine("Account Created Successfully");
        }

        public static void WelcomeMsg()
        {
            Console.WriteLine("Welcome to ALPHA BANK");
        }

        public static void InvalidOptionMsg()
        {
            Console.WriteLine("Invalid Option");
        }

        public static void InvalidAmountMsg()
        {
            Console.WriteLine("Invalid Amount");
        }

        public static void WrongPinMsg()
        {
            Console.WriteLine("Wrong PIN! Authentication Failed");
        }

        public static void TransferSuccess()
        {
            Console.WriteLine("Amount Transfered Successfully");
        }

        public static void TransferFailed()
        {
            Console.WriteLine("Transfer Failed");
        }

        public static void DepositSuccess()
        {
            Console.WriteLine("Amount Deposited Successfully");
        }

        public static void WithdrawSuccess()
        {
            Console.WriteLine("Amount Withdrawn Successfully");
        }
    }
}
