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
            Console.WriteLine("Welcome to Banking Service");
        }

        public void InvalidOptionMsg()
        {
            Console.WriteLine("Invalid Option");
        }

        public void InvalidAmountMsg()
        {
            Console.WriteLine("Invalid Amount");
        }

        public void WrongPasswordMsg()
        {
            Console.WriteLine("Wrong Password! Authentication Failed");
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

        public void BankCreationFailedMsg()
        {
            Console.WriteLine("Bank Creation Failed");
        }

        public void BankCreationSuccess()
        {
            Console.WriteLine("Bank Created Successfully");
        }

        public void BankNameExistsMsg()
        {
            Console.WriteLine("Bank Name Already Exists");
        }

        public void BankDoesnotExistMsg()
        {
            Console.WriteLine("Bank Doesnot Exist");
        }

        public void UsernameAlreadyExists()
        {
            Console.WriteLine("Account with given username already exists");
        }

        public void NoBanksMsg()
        {
            Console.WriteLine("Currently there are no banks.\nTry Creating a new bank.");
        }

        public void AccessDeniedMsg()
        {
            Console.WriteLine("User don't have access to do this operation");
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

        public void CurrencyDoesNotExist()
        {
            Console.WriteLine("Currency doesn't exist with that name");
        }

        public void CurrencyAlreadyExists()
        {
            Console.WriteLine("Currency with that name already exists");
        }

        public void TransactionNotFound()
        {
            Console.WriteLine("No valid transaction found");
        }

        public void RevertTransactionSuccess()
        {
            Console.WriteLine("Transaction Reverted Successfully");
        }

        public void NoTransactions()
        {
            Console.WriteLine("There are no Transactions in this account");
        }

        public void NoEmployeeActions()
        {
            Console.WriteLine("There are no actions for this employee");
        }
    }
}
