using ATM.Models;
using ATM.Services;
using System.Collections.Generic;

namespace ATM.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            StandardMessages.WelcomeMsg();
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
                        StandardMessages.AccountCreationSuccess();
                    }
                    catch (AccountCreationFailedException)
                    {
                        StandardMessages.AccountCreationFailed();
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
                            StandardMessages.InvalidOptionMsg();
                            break;
                        }
                        string userInputPin = ConsoleUI.GetPinFromUser();
                        try
                        {
                            bankManager.Authenticate(selectedAcc, userInputPin);
                        }
                        catch (AuthenticationFailedException)
                        {
                            StandardMessages.WrongPinMsg();
                            break;
                        }
                        string operation = ConsoleUI.SelectOperation();
                        if (operation == "1")
                        {
                            decimal amount = ConsoleUI.GetAmount('d');
                            try
                            {
                                bankManager.Deposit(selectedAcc, amount);
                                StandardMessages.DepositSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                StandardMessages.InvalidAmountMsg();
                            }
                            break;
                        }
                        else if (operation == "2")
                        {
                            decimal amount = ConsoleUI.GetAmount('w');
                            try
                            {
                                bankManager.Withdraw(selectedAcc, amount);
                                StandardMessages.WithdrawSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                StandardMessages.InvalidAmountMsg();
                            }
                            break;
                        }
                        else if (operation == "3")
                        {
                            decimal amount = ConsoleUI.GetAmount('t');
                            Account transferToAccount = ConsoleUI.SelectTransferToAccount(selectedAcc.accountNumber, allAccounts);
                            try
                            {
                                bankManager.Transfer(selectedAcc, transferToAccount, amount);
                                StandardMessages.TransferSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                StandardMessages.InvalidAmountMsg();
                            }
                            catch (TransferFailedException)
                            {
                                StandardMessages.TransferFailed();
                            }
                            break;
                        }
                        else if (operation == "4")
                        {
                            List<Transaction> transactions = bankManager.GetTransactions(selectedAcc);
                            ConsoleUI.PrintTransactions(transactions, selectedAcc.availableBalance);
                            break;
                        }
                        else
                        {
                            StandardMessages.InvalidOptionMsg();
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
                    StandardMessages.InvalidOptionMsg();
                }
            }
        }
    }
}
