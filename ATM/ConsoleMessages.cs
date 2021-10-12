using System;

namespace ATM.Services
{
    public class ConsoleMessages
    {
        public void AccountCreationFailed()
        {
            Console.WriteLine("Account Creation Failed");
        }

        public void AccountCreationSuccess()
        {
            Console.WriteLine("Account Created Successfully");
        }

        public void WelcomeMsg()
        {
            Console.WriteLine("Welcome to ALPHA BANK");
        }

        public void InvalidOptionMsg()
        {
            Console.WriteLine("Invalid Option");
        }

        public void InvalidAmountMsg()
        {
            Console.WriteLine("Invalid Amount");
        }

        public void WrongPinMsg()
        {
            Console.WriteLine("Wrong PIN! Authentication Failed");
        }

        public void TransferSuccess()
        {
            Console.WriteLine("Amount Transfered Successfully");
        }

        public void TransferFailed()
        {
            Console.WriteLine("Transfer Failed");
        }

        public void DepositSuccess()
        {
            Console.WriteLine("Amount Deposited Successfully");
        }

        public void WithdrawSuccess()
        {
            Console.WriteLine("Amount Withdrawn Successfully");
        }

        public void UserNotFoundMsg()
        {
            Console.WriteLine("User Not Found");
        }
    }
}
