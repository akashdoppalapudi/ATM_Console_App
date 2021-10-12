using ATM.Models;
using ATM.Services;
using System.Collections.Generic;

namespace ATM.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleUI consoleUI = new ConsoleUI();
            ConsoleMessages consoleMessages = new ConsoleMessages();
            consoleMessages.WelcomeMsg();
            while (true)
            {
                BankManager bankManager = new BankManager();
                string option = consoleUI.ExistingOrCreate();
                if (option == "1")
                {
                    (string name, string pin, AccountType accountType) = consoleUI.GetDataForAccountCreation();
                    try
                    {
                        bankManager.CreateNewAccount(name, pin, accountType);
                        consoleMessages.AccountCreationSuccess();
                    }
                    catch (AccountCreationFailedException)
                    {
                        consoleMessages.AccountCreationFailed();
                    }
                }
                else if (option == "2")
                {
                    while (true)
                    {
                        List<string> allAccountNames = bankManager.GetAllAccounts();
                        int selectedAccountId = consoleUI.SelectAccount(allAccountNames);
                        try
                        {
                            bankManager.CheckAccountExistance(selectedAccountId);
                        }
                        catch (UserNotFoundException)
                        {
                            consoleMessages.UserNotFoundMsg();
                            break;
                        }
                        string userInputPin = consoleUI.GetPinFromUser();
                        try
                        {
                            bankManager.Authenticate(selectedAccountId, userInputPin);
                        }
                        catch (AuthenticationFailedException)
                        {
                            consoleMessages.WrongPinMsg();
                            break;
                        }
                        string operation = consoleUI.SelectOperation();
                        if (operation == "1")
                        {
                            decimal amount = consoleUI.GetAmount('d');
                            try
                            {
                                bankManager.Deposit(selectedAccountId, amount);
                                consoleMessages.DepositSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                consoleMessages.InvalidAmountMsg();
                            }
                            break;
                        }
                        else if (operation == "2")
                        {
                            decimal amount = consoleUI.GetAmount('w');
                            try
                            {
                                bankManager.Withdraw(selectedAccountId, amount);
                                consoleMessages.WithdrawSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                consoleMessages.InvalidAmountMsg();
                            }
                            break;
                        }
                        else if (operation == "3")
                        {
                            decimal amount = consoleUI.GetAmount('t');
                            int transferToAccountId = consoleUI.SelectTransferToAccountId(selectedAccountId, allAccountNames);
                            try
                            {
                                bankManager.CheckAccountExistance(transferToAccountId);
                            }
                            catch (UserNotFoundException)
                            {
                                consoleMessages.UserNotFoundMsg();
                            }
                            try
                            {
                                bankManager.Transfer(selectedAccountId, transferToAccountId, amount);
                                consoleMessages.TransferSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                consoleMessages.InvalidAmountMsg();
                            }
                            catch (TransferFailedException)
                            {
                                consoleMessages.TransferFailed();
                            }
                            break;
                        }
                        else if (operation == "4")
                        {
                            List<Transaction> transactions = bankManager.GetTransactions(selectedAccountId);
                            decimal balance = bankManager.GetBalance(selectedAccountId);
                            consoleUI.PrintTransactions(transactions, balance);
                            break;
                        }
                        else
                        {
                            consoleMessages.InvalidOptionMsg();
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
                    consoleMessages.InvalidOptionMsg();
                }
            }
        }
    }
}
