using ATM.Models;
using ATM.Models.Enums;
using ATM.Services;
using ATM.Services.Exceptions;
using ATM.Services.IServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ATM.CLI
{
    class Program
    {
        // use static class for DIContainerBuilder
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
                        bankService.AddBank(newBank, adminEmployee);
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
                    try
                    {
                        bankService.CheckBankExistance(bankId);
                    }
                    catch (BankDoesnotExistException)
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
                                employeeService.Authenticate(bankId, employeeId, password);
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
                            if (employeeService.IsEmployeeAdmin(bankId, employeeId))
                            {
                                while (true)
                                {
                                    AdminOperation option3 = consoleUI.AdminOptions();
                                    if (option3 == AdminOperation.CreateEmployee)
                                    {
                                        Employee newEmployee;
                                        try
                                        {
                                            newEmployee = consoleUI.GetDataForEmployeeCreation(bankId);
                                            bankService.AddEmployee(bankId, employeeId, newEmployee);
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
                                        string updateEmployeeId;
                                        Employee currentEmployee, updatedEmployee;
                                        try
                                        {
                                            updateEmployeeId = employeeService.GetEmployeeIdByUsername(bankId, selectedUsername);
                                            currentEmployee = employeeService.GetEmployeeDetails(bankId, updateEmployeeId);
                                            updatedEmployee = consoleUI.GetDataForEmployeeUpdate(currentEmployee);
                                            bankService.UpdateEmployee(bankId, employeeId, updateEmployeeId, updatedEmployee);
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
                                        string deleteEmployeeId;
                                        try
                                        {
                                            deleteEmployeeId = employeeService.GetEmployeeIdByUsername(bankId, selectedUsername);
                                            bankService.DeleteEmployee(bankId, employeeId, deleteEmployeeId);
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
                                        Account newAccount;
                                        try
                                        {
                                            newAccount = consoleUI.GetDataForAccountCreation(bankId);
                                            bankService.AddAccount(bankId, employeeId, newAccount);
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
                                        string updateAccountId;
                                        Account currentAccount, updatedAccount;
                                        try
                                        {
                                            updateAccountId = accountService.GetAccountIdByUsername(bankId, selecteUsername);
                                            currentAccount = accountService.GetAccountDetails(bankId, updateAccountId);
                                            updatedAccount = consoleUI.GetDataForAccountUpdate(currentAccount);
                                            bankService.UpdateAccount(bankId, employeeId, updateAccountId, updatedAccount);
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
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        bankService.DeleteAccount(bankId, employeeId, deleteAccountId);
                                        consoleMessages.AccountDeleteSuccess();
                                    }
                                    else if (option3 == AdminOperation.AddCurrency)
                                    {
                                        try
                                        {
                                            Currency newCurrency = consoleUI.GetDataForCurrencyCreation(bankId);
                                            bankService.AddCurrency(bankId, employeeId, newCurrency);
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
                                            bankService.UpdateCurrency(bankId, employeeId, currencyName, updatedCurrency);
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
                                            currencyService.CheckCurrencyExistance(bankId, currencyName);
                                            bankService.DeleteCurrency(bankId, employeeId, currencyName);
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
                                        Bank currentBank = bankService.GetBankDetails(bankId);
                                        try
                                        {
                                            Bank updatedBank = consoleUI.GetDataForBankUpdate(currentBank);
                                            bankService.UpdateBank(bankId, employeeId, updatedBank);
                                            consoleMessages.BankUpdateSuccess();
                                            goto EndOfProgram;
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
                                            bankService.DeleteBank(bankId, employeeId);
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
                                            bankService.RevertTransaction(bankId, employeeId, txnId);
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
                                            IList<Transaction> transactions = transactionService.GetTransactions(bankId, selectedAccountId);
                                            decimal balance = accountService.GetBalance(bankId, selectedAccountId);
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
                                            IList<EmployeeAction> actions = employeeActionService.GetEmployeeActions(bankId, selectedEmployeeId);
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
                                        Account newAccount;
                                        try
                                        {
                                            newAccount = consoleUI.GetDataForAccountCreation(bankId);
                                            bankService.AddAccount(bankId, employeeId, newAccount);
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
                                        string updateAccountId;
                                        Account currentAccount, updatedAccount;
                                        try
                                        {
                                            updateAccountId = accountService.GetAccountIdByUsername(bankId, selecteUsername);
                                            currentAccount = accountService.GetAccountDetails(bankId, updateAccountId);
                                            updatedAccount = consoleUI.GetDataForAccountUpdate(currentAccount);
                                            bankService.UpdateAccount(bankId, employeeId, updateAccountId, updatedAccount);
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
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        bankService.DeleteAccount(bankId, employeeId, deleteAccountId);
                                        consoleMessages.AccountDeleteSuccess();
                                    }
                                    else if (option4 == StaffOperation.RevertTransaction)
                                    {
                                        string txnId = consoleUI.GetRevertTransactionId();
                                        try
                                        {
                                            bankService.RevertTransaction(bankId, employeeId, txnId);
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
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        IList<Transaction> transactions = transactionService.GetTransactions(bankId, selectedAccountId);
                                        decimal balance = accountService.GetBalance(bankId, selectedAccountId);
                                        consoleUI.PrintTransactions(transactions, balance);
                                    }
                                    else if (option4 == StaffOperation.ActionHistory)
                                    {
                                        IList<EmployeeAction> actions = employeeActionService.GetEmployeeActions(bankId, employeeId);
                                        consoleUI.PrintEmployeeActions(actions);
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
                                accountService.Authenticate(bankId, accountId, password);
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
                                        bankService.Deposit(bankId, accountId, currency, amount);
                                        consoleMessages.DepositSuccess();
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
                                        bankService.Withdraw(bankId, accountId, amount);
                                        consoleMessages.WithdrawSuccess();
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
                                    string toBankId, toAccountId;
                                    string selectedToBankId = consoleUI.SelectBank(bankNames);
                                    try
                                    {
                                        bankService.CheckBankExistance(selectedToBankId);
                                        toBankId = selectedToBankId;
                                        string selectedToUsername = consoleUI.GetUsername();
                                        toAccountId = accountService.GetAccountIdByUsername(toBankId, selectedToUsername);
                                        bankService.Transfer(bankId, accountId, toBankId, toAccountId, amount);
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
                                    accountService.CheckAccountExistance(bankId, accountId);
                                    IList<Transaction> transactions = transactionService.GetTransactions(bankId, accountId);
                                    decimal balance = accountService.GetBalance(bankId, accountId);
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
