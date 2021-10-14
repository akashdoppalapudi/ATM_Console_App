using ATM.Models;
using ATM.Models.Enums;
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
            BankManager bankManager;
            consoleMessages.WelcomeMsg();
            while (true)
            {
                bankManager = new BankManager();
                Option option1 = consoleUI.SelectOrCreateBank();
                if (option1 == Option.CreateNewBank)
                {
                    string bankName = consoleUI.GetBankName();
                    try
                    {
                        bankManager.CreateNewBank(bankName);
                        consoleMessages.BankCreationSuccess();
                    }
                    catch (BankCreationFailedException)
                    {
                        consoleMessages.BankCreationFailedMsg();
                    }
                    catch (BankNameAlreadyExistsException)
                    {
                        consoleMessages.BankNameExistsMsg();
                        consoleMessages.BankCreationFailedMsg();
                    }
                }
                else if (option1 == Option.SelectBank)
                {
                    Dictionary<string, string> bankNames = bankManager.GetBankNames();
                    string bankId = consoleUI.SelectBank(bankNames);
                    try
                    {
                        bool bankExistance = bankManager.CheckBankExistance(bankId);
                    }
                    catch (BankDoesnotExistException)
                    {
                        consoleMessages.BankDoesnotExistMsg();
                        continue;
                    }
                    Option option2 = consoleUI.ExistingOrCreate();
                    if (option2 == Option.CreateNewAccount)
                    {
                        (string name, string pin, string username, Gender gender, AccountType accountType) = consoleUI.GetDataForAccountCreation();
                        try
                        {
                            bankManager.CreateNewAccount(bankId, name, gender, pin, username, accountType);
                            consoleMessages.AccountCreationSuccess();
                        }
                        catch (AccountCreationFailedException)
                        {
                            consoleMessages.AccountCreationFailed();
                        }
                        catch (UsernameAlreadyExistsException)
                        {
                            consoleMessages.UsernameAlreadyExists();
                            consoleMessages.AccountCreationFailed();
                        }
                    }
                    else if (option2 == Option.ExistingUser)
                    {
                        string accountId;
                        string username = consoleUI.GetUsername();
                        try
                        {
                            accountId = bankManager.CheckAccountExistance(bankId, username);
                        }
                        catch (UserNotFoundException)
                        {
                            consoleMessages.UserNotFoundMsg();
                            continue;
                        }
                        string userInputPin = consoleUI.GetPinFromUser();
                        try
                        {
                            bankManager.Authenticate(bankId, accountId, userInputPin);
                        }
                        catch (AuthenticationFailedException)
                        {
                            consoleMessages.WrongPinMsg();
                            continue;
                        }
                        Option operation = consoleUI.SelectOperation();
                        if (operation == Option.Deposit)
                        {
                            decimal amount = consoleUI.GetAmount('d');
                            try
                            {
                                bankManager.Deposit(bankId, accountId, amount);
                                consoleMessages.DepositSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                consoleMessages.InvalidAmountMsg();
                            }
                            continue;
                        }
                        else if (operation == Option.Withdraw)
                        {
                            decimal amount = consoleUI.GetAmount('w');
                            try
                            {
                                bankManager.Withdraw(bankId, accountId, amount);
                                consoleMessages.WithdrawSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                consoleMessages.InvalidAmountMsg();
                            }
                            continue;
                        }
                        else if (operation == Option.Transfer)
                        {
                            decimal amount = consoleUI.GetAmount('t');
                            string toBankId = consoleUI.SelectBank(bankNames);
                            string toAccountId = null;
                            try
                            {
                                bool toBankExists = bankManager.CheckBankExistance(toBankId);
                            }
                            catch (BankDoesnotExistException)
                            {
                                consoleMessages.BankDoesnotExistMsg();
                            }
                            string transferToUsername = consoleUI.GetTransferToUsername();
                            try
                            {
                                toAccountId = bankManager.CheckAccountExistance(toBankId, transferToUsername);
                            }
                            catch (UserNotFoundException)
                            {
                                consoleMessages.UserNotFoundMsg();
                                consoleMessages.TransferFailed();
                            }
                            try
                            {
                                bankManager.Transfer(bankId, accountId, toBankId, toAccountId, amount);
                                consoleMessages.TransferSuccess();
                            }
                            catch (InvalidAmountException)
                            {
                                consoleMessages.InvalidAmountMsg();
                                consoleMessages.TransferFailed();
                            }
                            catch (TransferFailedException)
                            {
                                consoleMessages.TransferFailed();
                            }
                            continue;
                        }
                        else if (operation == Option.TransactionHistory)
                        {
                            List<Transaction> transactions = bankManager.GetTransactions(bankId, accountId);
                            decimal balance = bankManager.GetBalance(bankId, accountId);
                            consoleUI.PrintTransactions(transactions, balance);
                            continue;
                        }
                        else
                        {
                            consoleMessages.InvalidOptionMsg();
                            continue;
                        }

                    }
                    else
                    {
                        consoleMessages.InvalidOptionMsg();
                    }
                }
                else if (option1 == Option.Exit)
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
