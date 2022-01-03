using System;

namespace ATM.Services
{
    public interface IConsoleMessages
    {
        void AccountCreationSuccess();
        void AccountDeleteSuccess();
        void AccountUpdateSuccess();
        void BankCreationFailedMsg();
        void BankCreationSuccess();
        void BankDeleteSuccess();
        void BankDoesnotExistMsg();
        void BankUpdateSuccess();
        void CurrencyAddedSuccess();
        void CurrencyDeleteSuccess();
        void CurrencyUpdateSuccess();
        void DepositSuccess();
        void EmployeeDeleteSuccess();
        void EmployeeUpdateSuccess();
        void InvalidOptionMsg();
        void RevertTransactionSuccess();
        void TransferSuccess();
        void WelcomeMsg();
        void WithdrawSuccess();
        void Log(string msg);
    }

    public class ConsoleMessages : IConsoleMessages
    {
        public void AccountCreationSuccess()
        {
            Console.WriteLine("Account Created Successfully");
        }

        public void WelcomeMsg()
        {
            Console.WriteLine("Welcome to Banking Service");
        }

        public void InvalidOptionMsg()
        {
            Console.WriteLine("Invalid Option");
        }

        public void TransferSuccess()
        {
            Console.WriteLine("Amount Transfered Successfully");
        }

        public void DepositSuccess()
        {
            Console.WriteLine("Amount Deposited Successfully");
        }

        public void WithdrawSuccess()
        {
            Console.WriteLine("Amount Withdrawn Successfully");
        }

        public void BankCreationFailedMsg()
        {
            Console.WriteLine("Bank Creation Failed");
        }

        public void BankCreationSuccess()
        {
            Console.WriteLine("Bank Created Successfully");
        }

        public void BankDoesnotExistMsg()
        {
            Console.WriteLine("Bank Doesnot Exist");
        }

        public void EmployeeUpdateSuccess()
        {
            Console.WriteLine("Employee Updated Successfully");
        }

        public void EmployeeDeleteSuccess()
        {
            Console.WriteLine("Employee Deleted Successfully");
        }

        public void AccountUpdateSuccess()
        {
            Console.WriteLine("Account Updated Successfully");
        }

        public void AccountDeleteSuccess()
        {
            Console.WriteLine("Account Deleted Successfully");
        }

        public void BankUpdateSuccess()
        {
            Console.WriteLine("Bank Updated Successfully");
        }

        public void BankDeleteSuccess()
        {
            Console.WriteLine("Bank Deleted Successfully");
        }

        public void CurrencyAddedSuccess()
        {
            Console.WriteLine("Currency Added Successfully");
        }

        public void CurrencyUpdateSuccess()
        {
            Console.WriteLine("Currency Updated Successfully");
        }

        public void CurrencyDeleteSuccess()
        {
            Console.WriteLine("Currency Deleted Successfully");
        }

        public void RevertTransactionSuccess()
        {
            Console.WriteLine("Transaction Reverted Successfully");
        }

        public void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
