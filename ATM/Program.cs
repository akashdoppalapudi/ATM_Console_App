using ATM.Models.Enums;
using ATM.Models;
using ATM.Services;
using ATM.Services.Exceptions;
using System;
using System.Collections.Generic;

namespace ATM.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleUI consoleUI = new ConsoleUI();
            ConsoleMessages consoleMessages = new ConsoleMessages();
            BankService bankService;
            consoleMessages.WelcomeMsg();
            while (true)
            {
                bankService = new BankService();
                Option1 option1 = consoleUI.SelectOrCreateBank();
                if (option1 == Option1.CreateNewBank)
                {
                    (Tuple<string> bankDetails, Tuple<string, Gender, string, string> employeeDetails) = consoleUI.GetDataForBankCreation();
                    try
                    {
                        string bankId = bankService.CreateNewBank(bankDetails, employeeDetails);
                        consoleMessages.BankCreationSuccess();
                    }
                    catch (BankCreationFailedException)
                    {
                        consoleMessages.BankCreationFailedMsg();
                    }
                    catch (AccountCreationFailedException)
                    {
                        consoleMessages.AccountCreationFailed();
                        consoleMessages.BankCreationFailedMsg();
                    }
                    catch (BankNameAlreadyExistsException)
                    {
                        consoleMessages.BankNameExistsMsg();
                        consoleMessages.BankCreationFailedMsg();
                    }
                }
                else if (option1 == Option1.SelectBank)
                {

                    Dictionary<string, string> bankNames = bankService.GetAllBankNames();
                    string selectedBankId = consoleUI.SelectBank(bankNames);
                    string bankId;
                    try
                    {
                        bankId = bankService.CheckBankExistance(selectedBankId);
                    }
                    catch (BankDoesnotExistException)
                    {
                        consoleMessages.BankDoesnotExistMsg();
                        continue;
                    }
                    while (true)
                    {
                        Option2 option2 = consoleUI.UserOrStaff();
                        if (option2 == Option2.StaffLogin) { }
                        else if (option2 == Option2.UserLogin)
                        {
                            string username, password, accountId;
                            username = consoleUI.GetUsername();
                            try
                            {
                                accountId = bankService.CheckAccountExistance(bankId, username);
                            }
                            catch (AccountDoesNotExistException)
                            {
                                consoleMessages.UserNotFoundMsg();
                                continue;
                            }
                            password = consoleUI.GetPasswordFromUser();
                            try
                            {
                                bankService.AuthenticateUser(bankId, accountId, password);
                            }
                            catch (AuthenticationFailedException)
                            {
                                consoleMessages.WrongPasswordMsg();
                                continue;
                            }
                            while (true)
                            {
                                Option5 option5 = consoleUI.UserOptions();
                                decimal amount;
                                string txnId;
                                if (option5 == Option5.Deposit)
                                {
                                    amount = consoleUI.GetAmount('d');
                                    try
                                    {
                                        txnId = bankService.Deposit(bankId, accountId, amount);
                                        consoleMessages.DepositSuccess();
                                    }
                                    catch (InvalidAmountException)
                                    {
                                        consoleMessages.InvalidAmountMsg();
                                        continue;
                                    }
                                }
                                else if (option5 == Option5.Withdraw)
                                {
                                    amount = consoleUI.GetAmount('w');
                                    try
                                    {
                                        txnId = bankService.Withdraw(bankId, accountId, amount);
                                        consoleMessages.WithdrawSuccess();
                                    }
                                    catch (InvalidAmountException)
                                    {
                                        consoleMessages.InvalidAmountMsg();
                                        continue;
                                    }
                                }
                                else if (option5 == Option5.Transfer)
                                {
                                    amount = consoleUI.GetAmount('t');
                                    string toBankId, toAccountId;
                                    string selectedToBankId = consoleUI.SelectBank(bankNames);
                                    try
                                    {
                                        toBankId = bankService.CheckBankExistance(selectedToBankId);
                                    }
                                    catch(BankDoesnotExistException)
                                    {
                                        consoleMessages.BankDoesnotExistMsg();
                                        continue;
                                    }
                                    string selectedToUsername = consoleUI.GetUsername();
                                    try
                                    {
                                        toAccountId = bankService.CheckAccountExistance(toBankId, selectedToUsername);
                                    }
                                    catch (AccountDoesNotExistException)
                                    {
                                        consoleMessages.UserNotFoundMsg();
                                        continue;
                                    }
                                    try
                                    {
                                        txnId = bankService.Transfer(bankId, accountId, toBankId, toAccountId, amount);
                                        consoleMessages.TransferSuccess();
                                    }
                                    catch (InvalidAmountException)
                                    {
                                        consoleMessages.InvalidAmountMsg();
                                        continue;
                                    }
                                    catch (AccessDeniedException)
                                    {
                                        consoleMessages.TransferFailed();
                                        continue;
                                    }
                                }
                                else if (option5 == Option5.TransactionHistory)
                                {
                                    List<Transaction> transactions = bankService.GetTransactions(bankId, accountId);
                                    decimal balance = bankService.GetBalance(bankId, accountId);
                                }
                                else if (option5 == Option5.Back)
                                {
                                    break;
                                }
                                else
                                {
                                    consoleMessages.InvalidOptionMsg();
                                }
                            }
                        }
                        else if (option2 == Option2.Back)
                        {
                            break;
                        }
                        else
                        {
                            consoleMessages.InvalidOptionMsg();
                        }
                    }
                    
                }
                else if (option1 == Option1.Exit)
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
