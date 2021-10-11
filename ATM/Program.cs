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
                        List<Account> allAccounts = bankManager.GetAllAccounts();
                        Account selectedAcc = ConsoleUI.SelectAccount(allAccounts);
                        if (selectedAcc == null)
                        {
                            ConsoleMessages.InvalidOptionMsg();
                            break;
                        }
                        string userInputPin = ConsoleUI.GetPinFromUser();
                        try
                        {
                            bankManager.Authenticate(selectedAcc, userInputPin);
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
                                bankManager.Deposit(selectedAcc, amount);
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
                                bankManager.Withdraw(selectedAcc, amount);
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
                            Account transferToAccount = ConsoleUI.SelectTransferToAccount(selectedAcc.AccountId, allAccounts);
                            try
                            {
                                bankManager.Transfer(selectedAcc, transferToAccount, amount);
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
                            List<Transaction> transactions = bankManager.GetTransactions(selectedAcc);
                            ConsoleUI.PrintTransactions(transactions, selectedAcc.Balance);
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
