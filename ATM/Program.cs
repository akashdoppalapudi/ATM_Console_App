using ATM.Models;
using ATM.Services;
using System.Collections.Generic;

namespace ATM.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleMessages.WelcomeMsg();
            while (true)
            {
                BankManager bankManager = new BankManager();
                string option = ConsoleUI.ExistingOrCreate();
                if (option == "1")
                {
                    (string name, string pin, AccountType accountType) = ConsoleUI.GetDataForAccountCreation();
                    try
                    {
                        bankManager.CreateNewAccount(name, pin, accountType);
                        ConsoleMessages.AccountCreationSuccess();
                    }
                    catch (AccountCreationFailedException)
                    {
                        ConsoleMessages.AccountCreationFailed();
                    }
                }
                else if (option == "2")
                {
                    while (true)
                    {
                        List<string> allAccountNames = bankManager.GetAllAccounts();
                        int selectedAccountId = ConsoleUI.SelectAccount(allAccountNames);
                        try
                        {
                            bankManager.CheckAccountExistance(selectedAccountId);
                        }
                        catch (UserNotFoundException)
                        {
                            ConsoleMessages.UserNotFoundMsg();
                            break;
                        }
                        string userInputPin = ConsoleUI.GetPinFromUser();
                        try
                        {
                            bankManager.Authenticate(selectedAccountId, userInputPin);
                        }
                        catch (AuthenticationFailedException)
                        {
                            ConsoleMessages.WrongPinMsg();
                            break;
                        }
                        string operation = ConsoleUI.SelectOperation();
                        if (operation == "1")
                        {
                            decimal amount = ConsoleUI.GetAmount('d');
                            try
                            {
                                bankManager.Deposit(selectedAccountId, amount);
                                ConsoleMessages.DepositSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                ConsoleMessages.InvalidAmountMsg();
                            }
                            break;
                        }
                        else if (operation == "2")
                        {
                            decimal amount = ConsoleUI.GetAmount('w');
                            try
                            {
                                bankManager.Withdraw(selectedAccountId, amount);
                                ConsoleMessages.WithdrawSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                ConsoleMessages.InvalidAmountMsg();
                            }
                            break;
                        }
                        else if (operation == "3")
                        {
                            decimal amount = ConsoleUI.GetAmount('t');
                            int transferToAccountId = ConsoleUI.SelectTransferToAccountId(selectedAccountId, allAccountNames);
                            try
                            {
                                bankManager.CheckAccountExistance(transferToAccountId);
                            }
                            catch (UserNotFoundException)
                            {
                                ConsoleMessages.UserNotFoundMsg();
                            }
                            try
                            {
                                bankManager.Transfer(selectedAccountId, transferToAccountId, amount);
                                ConsoleMessages.TransferSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                ConsoleMessages.InvalidAmountMsg();
                            }
                            catch (TransferFailedException)
                            {
                                ConsoleMessages.TransferFailed();
                            }
                            break;
                        }
                        else if (operation == "4")
                        {
                            List<Transaction> transactions = bankManager.GetTransactions(selectedAccountId);
                            decimal balance = bankManager.GetBalance(selectedAccountId);
                            ConsoleUI.PrintTransactions(transactions, balance);
                            break;
                        }
                        else
                        {
                            ConsoleMessages.InvalidOptionMsg();
                            break;
                        }
                    }
                }
                else if (option == "e")
                {
                    break;
                }
                else
                {
                    ConsoleMessages.InvalidOptionMsg();
                }
            }
        }
    }
}
