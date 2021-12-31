using ATM.Models;
using ATM.Models.Enums;
using ATM.Models.ViewModels;
using ATM.Services;
using ATM.Services.Exceptions;
using ATM.Services.Extensions;
using ATM.Services.IServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ATM.CLI
{
    class Program
    {
        // follow the naming conventions when nameing a variable all time
        public static readonly IServiceProvider services = DIContainerBuilder.Build();
        static void Main(string[] args)
        {
            IConsoleMessages consoleMessages = services.GetService<IConsoleMessages>();
            IConsoleUI consoleUI = services.GetService<IConsoleUI>();
            IBankService bankService = services.GetService<IBankService>();
            IEmployeeService employeeService = services.GetService<IEmployeeService>();
            IAccountService accountService = services.GetService<IAccountService>();
            ICurrencyService currencyService = services.GetService<ICurrencyService>();
            ITransactionService transactionService = services.GetService<ITransactionService>();
            IEmployeeActionService employeeActionService = services.GetService<IEmployeeActionService>();

            consoleMessages.WelcomeMsg();
            while (true)
            {
                BankCreateOrSelect option1 = consoleUI.SelectOrCreateBank();
                if (option1 == BankCreateOrSelect.CreateNewBank)
                {
                    try
                    {
                        (Bank newBank, Employee adminEmployee) = consoleUI.GetDataForBankCreation();
                        bankService.AddBank(newBank);
                        employeeService.AddEmployee(adminEmployee);
                        Currency defaultCurrency = new Currency
                        {
                            Name = "INR",
                            ExchangeRate = 1,
                            BankId = newBank.Id,
                        };
                        currencyService.AddCurrency(defaultCurrency);
                        consoleMessages.BankCreationSuccess();
                    }
                    catch (BankNameAlreadyExistsException)
                    {
                        consoleMessages.BankNameExistsMsg();
                        consoleMessages.BankCreationFailedMsg();
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
                }
                else if (option1 == BankCreateOrSelect.SelectBank)
                {
                    Dictionary<string, string> bankNames = bankService.GetAllBankNames();
                    if (bankNames.Count == 0 || bankNames == null)
                    {
                        consoleMessages.NoBanksMsg();
                        continue;
                    }
                    string bankId = consoleUI.SelectBank(bankNames);
                    if (bankId == null)
                    {
                        consoleMessages.BankDoesnotExistMsg();
                        continue;
                    }
                    while (true)
                    {
                        StaffOrUserLogin option2 = consoleUI.UserOrStaff();
                        if (option2 == StaffOrUserLogin.StaffLogin)
                        {
                            string username, password, employeeId;
                            username = consoleUI.GetUsername();
                            try
                            {
                                employeeId = employeeService.GetEmployeeIdByUsername(bankId, username);
                                password = consoleUI.GetPasswordFromUser();
                                employeeService.Authenticate(employeeId, password);
                            }
                            catch (EmployeeDoesNotExistException)
                            {
                                consoleMessages.UserNotFoundMsg();
                                continue;
                            }
                            catch (AuthenticationFailedException)
                            {
                                consoleMessages.WrongPasswordMsg();
                                continue;
                            }
                            if (employeeService.IsEmployeeAdmin(employeeId))
                            {
                                while (true)
                                {
                                    AdminOperation option3 = consoleUI.AdminOptions();
                                    if (option3 == AdminOperation.CreateEmployee)
                                    {
                                        try
                                        {
                                            Employee newEmployee = consoleUI.GetDataForEmployeeCreation(bankId);
                                            newEmployee.BankId = bankId;
                                            employeeService.AddEmployee(newEmployee);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                AccountId = newEmployee.Id,
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.NewAccount,
                                                BankId = bankId,
                                                EmployeeId = employeeId
                                            };
                                            employeeActionService.AddEmployeeAction(action);
                                            consoleMessages.AccountCreationSuccess();
                                        }
                                        catch (AccountCreationFailedException)
                                        {
                                            consoleMessages.AccountCreationFailed();
                                            continue;
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
                                        }
                                        catch (UsernameAlreadyExistsException)
                                        {
                                            consoleMessages.UsernameAlreadyExists();
                                            continue;
                                        }
                                    }
                                    else if (option3 == AdminOperation.UpdateEmployee)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        try
                                        {
                                            string updateEmployeeId = employeeService.GetEmployeeIdByUsername(bankId, selectedUsername);
                                            EmployeeViewModel currentEmployee = employeeService.GetEmployeeDetails(updateEmployeeId);
                                            Employee updatedEmployee = consoleUI.GetDataForEmployeeUpdate(currentEmployee);
                                            employeeService.UpdateEmployee(updateEmployeeId, updatedEmployee);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                AccountId = updateEmployeeId,
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.UpdateAccount,
                                                BankId = bankId,
                                                EmployeeId = employeeId
                                            };
                                            employeeActionService.AddEmployeeAction(action);
                                            consoleMessages.EmployeeUpdateSuccess();
                                        }
                                        catch (EmployeeDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
                                        }
                                        catch (UsernameAlreadyExistsException)
                                        {
                                            consoleMessages.UsernameAlreadyExists();
                                            continue;
                                        }
                                    }
                                    else if (option3 == AdminOperation.DeleteEmployee)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        try
                                        {
                                            string deleteEmployeeId = employeeService.GetEmployeeIdByUsername(bankId, selectedUsername);
                                            employeeService.DeleteEmployee(deleteEmployeeId);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                AccountId = deleteEmployeeId,
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.DeleteAccount,
                                                EmployeeId = employeeId,
                                                BankId = bankId
                                            };
                                            employeeActionService.AddEmployeeAction(action);
                                            consoleMessages.EmployeeDeleteSuccess();
                                        }
                                        catch (EmployeeDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
                                        }
                                    }
                                    else if (option3 == AdminOperation.CreateAccount)
                                    {
                                        try
                                        {
                                            Account newAccount = consoleUI.GetDataForAccountCreation(bankId);
                                            newAccount.BankId = bankId;
                                            accountService.AddAccount(newAccount);
                                            Transaction transaction = new Transaction
                                            {
                                                Id = bankId.GenTransactionId(newAccount.Id),
                                                TransactionDate = DateTime.Now,
                                                TransactionType = TransactionType.Credit,
                                                BankId = bankId,
                                                AccountId = newAccount.Id,
                                                TransactionNarrative = TransactionNarrative.AccountCreation,
                                                TransactionAmount = 1500
                                            };
                                            transactionService.AddTransaction(transaction);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                TXNId = transaction.Id,
                                                AccountId = newAccount.Id,
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.NewAccount,
                                                BankId = bankId,
                                                EmployeeId = employeeId
                                            };
                                            employeeActionService.AddEmployeeAction(action);
                                            consoleMessages.AccountCreationSuccess();
                                        }
                                        catch (AccountCreationFailedException)
                                        {
                                            consoleMessages.AccountCreationFailed();
                                            continue;
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
                                        }

                                        catch (UsernameAlreadyExistsException)
                                        {
                                            consoleMessages.UsernameAlreadyExists();
                                            continue;
                                        }
                                    }
                                    else if (option3 == AdminOperation.UpdateAccount)
                                    {
                                        string selecteUsername = consoleUI.GetUsername();
                                        try
                                        {
                                            string updateAccountId = accountService.GetAccountIdByUsername(bankId, selecteUsername);
                                            AccountViewModel currentAccount = accountService.GetAccountDetails(updateAccountId);
                                            Account updatedAccount = consoleUI.GetDataForAccountUpdate(currentAccount);
                                            accountService.UpdateAccount(updateAccountId, updatedAccount);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                AccountId = updateAccountId,
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.UpdateAccount,
                                                EmployeeId = employeeId,
                                                BankId = bankId
                                            };
                                            employeeActionService.AddEmployeeAction(action);
                                            consoleMessages.AccountUpdateSuccess();
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        catch (UsernameAlreadyExistsException)
                                        {
                                            consoleMessages.UsernameAlreadyExists();
                                            continue;
                                        }
                                    }
                                    else if (option3 == AdminOperation.DeleteAccount)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string deleteAccountId;
                                        try
                                        {
                                            deleteAccountId = accountService.GetAccountIdByUsername(bankId, selectedUsername);
                                            accountService.DeleteAccount(deleteAccountId);
                                            EmployeeAction employeeAction = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                AccountId = deleteAccountId,
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.DeleteAccount,
                                                EmployeeId = employeeId,
                                                BankId = bankId
                                            };
                                            employeeActionService.AddEmployeeAction(employeeAction);
                                            consoleMessages.AccountDeleteSuccess();
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                    }
                                    else if (option3 == AdminOperation.AddCurrency)
                                    {
                                        try
                                        {
                                            Currency newCurrency = consoleUI.GetDataForCurrencyCreation(bankId);
                                            currencyService.AddCurrency(newCurrency);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.UpdateBank,
                                                EmployeeId = employeeId,
                                                BankId = bankId
                                            };
                                            employeeActionService.AddEmployeeAction(action);
                                            consoleMessages.CurrencyAddedSuccess();
                                        }
                                        catch (CurrencyAlreadyExistsException)
                                        {
                                            consoleMessages.CurrencyAlreadyExists();
                                            continue;
                                        }
                                        catch (CurrencyDataInvalidException) { }
                                    }
                                    else if (option3 == AdminOperation.ChangeCurrency)
                                    {
                                        try
                                        {
                                            string currencyName = consoleUI.GetCurrencyName();
                                            Currency currency = currencyService.GetCurrencyByName(bankId, currencyName);
                                            Currency updatedCurrency = consoleUI.GetDataForCurrencyUpdate(currency);
                                            currencyService.UpdateCurrency(bankId, currencyName, updatedCurrency);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.UpdateBank,
                                                EmployeeId = employeeId,
                                                BankId = bankId
                                            };
                                            employeeActionService.AddEmployeeAction(action);
                                            consoleMessages.CurrencyUpdateSuccess();
                                        }
                                        catch (CurrencyDoesNotExistException)
                                        {
                                            consoleMessages.CurrencyDoesNotExist();
                                            continue;
                                        }
                                    }
                                    else if (option3 == AdminOperation.RemoveCurrency)
                                    {
                                        string currencyName = consoleUI.GetCurrencyName();
                                        try
                                        {
                                            currencyService.DeleteCurrency(bankId, currencyName);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.UpdateBank,
                                                EmployeeId = employeeId,
                                                BankId = bankId
                                            };
                                            employeeActionService.AddEmployeeAction(action);
                                            consoleMessages.CurrencyDeleteSuccess();
                                        }
                                        catch (CurrencyDoesNotExistException)
                                        {
                                            consoleMessages.CurrencyDoesNotExist();
                                            continue;
                                        }
                                    }
                                    else if (option3 == AdminOperation.UpdateBank)
                                    {
                                        try
                                        {
                                            Bank currentBank = bankService.GetBankDetails(bankId);
                                            Bank updatedBank = consoleUI.GetDataForBankUpdate(currentBank);
                                            bankService.UpdateBank(bankId, updatedBank);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.UpdateBank,
                                                EmployeeId = employeeId,
                                                BankId = bankId
                                            };
                                            employeeActionService.AddEmployeeAction(action);
                                            consoleMessages.BankUpdateSuccess();
                                            goto EndOfProgram;
                                        }
                                        catch (BankDoesnotExistException)
                                        {
                                            consoleMessages.BankDoesnotExistMsg();
                                            continue;
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
                                        }
                                        catch (BankNameAlreadyExistsException)
                                        {
                                            consoleMessages.BankNameExistsMsg();
                                            continue;
                                        }
                                    }
                                    else if (option3 == AdminOperation.DeleteBank)
                                    {
                                        try
                                        {
                                            bankService.DeleteBank(bankId);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.DeleteBank,
                                                EmployeeId = employeeId,
                                                BankId = bankId
                                            };
                                            employeeActionService.AddEmployeeAction(action);
                                            consoleMessages.BankDeleteSuccess();
                                            goto EndOfProgram;
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
                                        }
                                    }
                                    else if (option3 == AdminOperation.RevertTransaction)
                                    {
                                        string txnId = consoleUI.GetRevertTransactionId();
                                        try
                                        {
                                            bankService.RevertTransaction(bankId, txnId);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.RevertTransaction,
                                                EmployeeId = employeeId,
                                                BankId = bankId
                                            };
                                            consoleMessages.RevertTransactionSuccess();
                                        }
                                        catch (TransactionNotFoundException)
                                        {
                                            consoleMessages.TransactionNotFound();
                                            continue;
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
                                        }
                                    }
                                    else if (option3 == AdminOperation.TransactionHistory)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string selectedAccountId;
                                        try
                                        {
                                            selectedAccountId = accountService.GetAccountIdByUsername(bankId, selectedUsername);
                                            IList<Transaction> transactions = transactionService.GetTransactions(selectedAccountId);
                                            decimal balance = accountService.GetBalance(selectedAccountId);
                                            consoleUI.PrintTransactions(transactions, balance);
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        catch (NoTransactionsException)
                                        {
                                            consoleMessages.NoTransactions();
                                        }

                                    }
                                    else if (option3 == AdminOperation.ActionHistory)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string selectedEmployeeId;
                                        try
                                        {
                                            selectedEmployeeId = employeeService.GetEmployeeIdByUsername(bankId, selectedUsername);
                                            IList<EmployeeAction> actions = employeeActionService.GetEmployeeActions(selectedEmployeeId);
                                            consoleUI.PrintEmployeeActions(actions);
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        catch (NoEmployeeActionsException)
                                        {
                                            consoleMessages.NoEmployeeActions();
                                            continue;
                                        }
                                    }
                                    else if (option3 == AdminOperation.Back)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        consoleMessages.InvalidOptionMsg();
                                    }
                                }
                            }
                            else
                            {
                                while (true)
                                {
                                    StaffOperation option4 = consoleUI.StaffOptions();
                                    if (option4 == StaffOperation.CreateAccount)
                                    {
                                        try
                                        {
                                            Account newAccount = consoleUI.GetDataForAccountCreation(bankId);
                                            newAccount.BankId = bankId;
                                            accountService.AddAccount(newAccount);
                                            Transaction transaction = new Transaction
                                            {
                                                Id = bankId.GenTransactionId(newAccount.Id),
                                                TransactionDate = DateTime.Now,
                                                TransactionType = TransactionType.Credit,
                                                BankId = bankId,
                                                AccountId = newAccount.Id,
                                                TransactionNarrative = TransactionNarrative.AccountCreation,
                                                TransactionAmount = 1500
                                            };
                                            transactionService.AddTransaction(transaction);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                TXNId = transaction.Id,
                                                AccountId = newAccount.Id,
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.NewAccount,
                                                BankId = bankId,
                                                EmployeeId = employeeId
                                            };
                                            employeeActionService.AddEmployeeAction(action);
                                            consoleMessages.AccountCreationSuccess();
                                        }
                                        catch (AccountCreationFailedException)
                                        {
                                            consoleMessages.AccountCreationFailed();
                                            continue;
                                        }
                                        catch (UsernameAlreadyExistsException)
                                        {
                                            consoleMessages.UsernameAlreadyExists();
                                            continue;
                                        }
                                    }
                                    else if (option4 == StaffOperation.UpdateAccount)
                                    {
                                        string selecteUsername = consoleUI.GetUsername();
                                        try
                                        {
                                            string updateAccountId = accountService.GetAccountIdByUsername(bankId, selecteUsername);
                                            AccountViewModel currentAccount = accountService.GetAccountDetails(updateAccountId);
                                            Account updatedAccount = consoleUI.GetDataForAccountUpdate(currentAccount);
                                            accountService.UpdateAccount(updateAccountId, updatedAccount);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                AccountId = updateAccountId,
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.UpdateAccount,
                                                EmployeeId = employeeId,
                                                BankId = bankId
                                            };
                                            employeeActionService.AddEmployeeAction(action);
                                            consoleMessages.AccountUpdateSuccess();
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        catch (UsernameAlreadyExistsException)
                                        {
                                            consoleMessages.UsernameAlreadyExists();
                                            continue;
                                        }
                                    }
                                    else if (option4 == StaffOperation.DeleteAccount)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string deleteAccountId;
                                        try
                                        {
                                            deleteAccountId = accountService.GetAccountIdByUsername(bankId, selectedUsername);
                                            accountService.DeleteAccount(deleteAccountId);
                                            EmployeeAction employeeAction = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                AccountId = deleteAccountId,
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.DeleteAccount,
                                                EmployeeId = employeeId,
                                                BankId = bankId
                                            };
                                            employeeActionService.AddEmployeeAction(employeeAction);
                                            consoleMessages.AccountDeleteSuccess();
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                    }
                                    else if (option4 == StaffOperation.RevertTransaction)
                                    {
                                        string txnId = consoleUI.GetRevertTransactionId();
                                        try
                                        {
                                            bankService.RevertTransaction(bankId, txnId);
                                            EmployeeAction action = new EmployeeAction
                                            {
                                                Id = bankId.GenEmployeeActionId(employeeId),
                                                ActionDate = DateTime.Now,
                                                ActionType = EmployeeActionType.RevertTransaction,
                                                EmployeeId = employeeId,
                                                BankId = bankId
                                            };
                                            consoleMessages.RevertTransactionSuccess();
                                        }
                                        catch (TransactionNotFoundException)
                                        {
                                            consoleMessages.TransactionNotFound();
                                            continue;
                                        }
                                    }
                                    else if (option4 == StaffOperation.TransactionHistory)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string selectedAccountId;
                                        try
                                        {
                                            selectedAccountId = accountService.GetAccountIdByUsername(bankId, selectedUsername);
                                            IList<Transaction> transactions = transactionService.GetTransactions(selectedAccountId);
                                            decimal balance = accountService.GetBalance(selectedAccountId);
                                            consoleUI.PrintTransactions(transactions, balance);
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        catch (NoTransactionsException)
                                        {
                                            consoleMessages.NoTransactions();
                                        }
                                    }
                                    else if (option4 == StaffOperation.ActionHistory)
                                    {
                                        try
                                        {
                                            IList<EmployeeAction> actions = employeeActionService.GetEmployeeActions(employeeId);
                                            consoleUI.PrintEmployeeActions(actions);
                                        }
                                        catch (NoEmployeeActionsException)
                                        {
                                            consoleMessages.NoEmployeeActions();
                                        }
                                    }
                                    else if (option4 == StaffOperation.Back)
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
                        else if (option2 == StaffOrUserLogin.UserLogin)
                        {
                            string username, password, accountId;
                            username = consoleUI.GetUsername();
                            try
                            {
                                accountId = accountService.GetAccountIdByUsername(bankId, username);
                                password = consoleUI.GetPasswordFromUser();
                                accountService.Authenticate(accountId, password);
                            }
                            catch (AccountDoesNotExistException)
                            {
                                consoleMessages.UserNotFoundMsg();
                                continue;
                            }
                            catch (AuthenticationFailedException)
                            {
                                consoleMessages.WrongPasswordMsg();
                                continue;
                            }
                            while (true)
                            {
                                UserOperation option5 = consoleUI.UserOptions();
                                decimal amount;
                                if (option5 == UserOperation.Deposit)
                                {
                                    amount = consoleUI.GetAmount('d');
                                    string currencyName = consoleUI.GetCurrencyName();
                                    try
                                    {
                                        Currency currency = currencyService.GetCurrencyByName(bankId, currencyName);
                                        accountService.Deposit(accountId, currency, amount);
                                        Transaction transaction = new Transaction
                                        {
                                            Id = bankId.GenTransactionId(accountId),
                                            TransactionDate = DateTime.Now,
                                            TransactionType = TransactionType.Credit,
                                            TransactionNarrative = TransactionNarrative.Deposit,
                                            TransactionAmount = amount,
                                            BankId = bankId,
                                            AccountId = accountId
                                        };
                                        transactionService.AddTransaction(transaction);
                                        consoleMessages.DepositSuccess();
                                    }
                                    catch (AccountDoesNotExistException)
                                    {
                                        consoleMessages.UserNotFoundMsg();
                                        continue;
                                    }
                                    catch (CurrencyDoesNotExistException)
                                    {
                                        consoleMessages.CurrencyDoesNotExist();
                                        continue;
                                    }
                                    catch (InvalidAmountException)
                                    {
                                        consoleMessages.InvalidAmountMsg();
                                        continue;
                                    }
                                }
                                else if (option5 == UserOperation.Withdraw)
                                {
                                    amount = consoleUI.GetAmount('w');
                                    try
                                    {
                                        accountService.Withdraw(accountId, amount);
                                        Transaction transaction = new Transaction
                                        {
                                            Id = bankId.GenTransactionId(accountId),
                                            TransactionDate = DateTime.Now,
                                            TransactionType = TransactionType.Debit,
                                            TransactionNarrative = TransactionNarrative.Withdraw,
                                            TransactionAmount = amount,
                                            BankId = bankId,
                                            AccountId = accountId
                                        };
                                        transactionService.AddTransaction(transaction);
                                        consoleMessages.WithdrawSuccess();
                                    }
                                    catch (AccountDoesNotExistException)
                                    {
                                        consoleMessages.UserNotFoundMsg();
                                        continue;
                                    }
                                    catch (InvalidAmountException)
                                    {
                                        consoleMessages.InvalidAmountMsg();
                                        continue;
                                    }
                                }
                                else if (option5 == UserOperation.Transfer)
                                {
                                    amount = consoleUI.GetAmount('t');
                                    string selectedToBankId = consoleUI.SelectBank(bankNames);
                                    try
                                    {
                                        string toBankId = selectedToBankId;
                                        string selectedToUsername = consoleUI.GetUsername();
                                        string toAccountId = accountService.GetAccountIdByUsername(toBankId, selectedToUsername);
                                        accountService.Transfer(accountId, toAccountId, amount);
                                        Transaction toTransaction = new Transaction
                                        {
                                            Id = bankId.GenTransactionId(accountId),
                                            TransactionDate = DateTime.Now,
                                            TransactionType = TransactionType.Debit,
                                            TransactionNarrative = TransactionNarrative.Transfer,
                                            TransactionAmount = amount,
                                            BankId = bankId,
                                            AccountId = accountId,
                                            ToAccountId = toAccountId,
                                            ToBankId = toBankId
                                        };
                                        transactionService.AddTransaction(toTransaction);
                                        Transaction fromTransaction = new Transaction
                                        {
                                            Id = bankId.GenTransactionId(toAccountId),
                                            TransactionDate = DateTime.Now,
                                            TransactionType = TransactionType.Credit,
                                            TransactionNarrative = TransactionNarrative.Transfer,
                                            TransactionAmount = amount,
                                            BankId = toBankId,
                                            AccountId = toAccountId,
                                        };
                                        transactionService.AddTransaction(fromTransaction);
                                        consoleMessages.TransferSuccess();
                                    }
                                    catch (BankDoesnotExistException)
                                    {
                                        consoleMessages.BankDoesnotExistMsg();
                                        continue;
                                    }
                                    catch (AccountDoesNotExistException)
                                    {
                                        consoleMessages.UserNotFoundMsg();
                                        continue;
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
                                else if (option5 == UserOperation.TransactionHistory)
                                {
                                    IList<Transaction> transactions = transactionService.GetTransactions(accountId);
                                    decimal balance = accountService.GetBalance(accountId);
                                    consoleUI.PrintTransactions(transactions, balance);
                                }
                                else if (option5 == UserOperation.Back)
                                {
                                    break;
                                }
                                else
                                {
                                    consoleMessages.InvalidOptionMsg();
                                }
                            }
                        }
                        else if (option2 == StaffOrUserLogin.Back)
                        {
                            break;
                        }
                        else
                        {
                            consoleMessages.InvalidOptionMsg();
                        }
                    }

                }
                else if (option1 == BankCreateOrSelect.Exit)
                {
                    break;
                }
                else
                {
                    consoleMessages.InvalidOptionMsg();
                }
            EndOfProgram:;
            }
        }
    }
}
