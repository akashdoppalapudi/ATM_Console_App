using ATM.Models;
using ATM.Models.Enums;
using ATM.Services;
using ATM.Services.Exceptions;
using System.Collections.Generic;

namespace ATM.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleUI consoleUI = new ConsoleUI();
            ConsoleMessages consoleMessages = new ConsoleMessages();
            EmployeeService employeeService = new EmployeeService();
            BankService bankService;
            consoleMessages.WelcomeMsg();
            while (true)
            {
                bankService = new BankService();
                BankCreateOrSelect option1 = consoleUI.SelectOrCreateBank();
                if (option1 == BankCreateOrSelect.CreateNewBank)
                {
                    try
                    {
                        (Bank newBank, Employee adminEmployee) = consoleUI.GetDataForBankCreation();
                        try
                        {
                            bankService.AddBank(newBank, adminEmployee);
                            consoleMessages.BankCreationSuccess();
                        }
                        catch (BankNameAlreadyExistsException)
                        {
                            consoleMessages.BankNameExistsMsg();
                            consoleMessages.BankCreationFailedMsg();
                        }
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
                            Employee employee;
                            username = consoleUI.GetUsername();
                            try
                            {
                                employeeId = bankService.GetEmployeeIdByUsername(bankId, username);
                                employee = bankService.GetEmployeeDetails(bankId, employeeId);
                            }
                            catch (EmployeeDoesNotExistException)
                            {
                                consoleMessages.UserNotFoundMsg();
                                continue;
                            }
                            password = consoleUI.GetPasswordFromUser();
                            try
                            {
                                bankService.AuthenticateEmployee(bankId, employeeId, password);
                            }
                            catch (AuthenticationFailedException)
                            {
                                consoleMessages.WrongPasswordMsg();
                                continue;
                            }
                            if (employeeService.IsEmployeeAdmin(employee))
                            {
                                while (true)
                                {
                                    AdminOperation option3 = consoleUI.AdminOptions();
                                    if (option3 == AdminOperation.CreateEmployee)
                                    {
                                        Employee newEmployee;
                                        try
                                        {
                                            newEmployee = consoleUI.GetDataForEmployeeCreation();
                                        }
                                        catch (AccountCreationFailedException)
                                        {
                                            consoleMessages.AccountCreationFailed();
                                            continue;
                                        }
                                        try
                                        {
                                            bankService.AddEmployee(bankId, employeeId, newEmployee);
                                            consoleMessages.AccountCreationSuccess();
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
                                            updateEmployeeId = bankService.GetEmployeeIdByUsername(bankId, selectedUsername);
                                        }
                                        catch (EmployeeDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        currentEmployee = bankService.GetEmployeeDetails(bankId, updateEmployeeId);
                                        updatedEmployee = consoleUI.GetDataForEmployeeUpdate(currentEmployee);
                                        try
                                        {
                                            bankService.UpdateEmployee(bankId, employeeId, updateEmployeeId, updatedEmployee);
                                            consoleMessages.EmployeeUpdateSuccess();
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
                                            deleteEmployeeId = bankService.GetEmployeeIdByUsername(bankId, selectedUsername);
                                        }
                                        catch (EmployeeDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        try
                                        {
                                            bankService.DeleteEmployee(bankId, employeeId, deleteEmployeeId);
                                            consoleMessages.EmployeeDeleteSuccess();
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
                                            newAccount = consoleUI.GetDataForAccountCreation();
                                        }
                                        catch (AccountCreationFailedException)
                                        {
                                            consoleMessages.AccountCreationFailed();
                                            continue;
                                        }
                                        try
                                        {
                                            bankService.AddAccount(bankId, employeeId, newAccount);
                                            consoleMessages.AccountCreationSuccess();
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
                                            updateAccountId = bankService.GetAccountIdByUsername(bankId, selecteUsername);
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        currentAccount = bankService.GetAccountDetails(bankId, updateAccountId);
                                        updatedAccount = consoleUI.GetDataForAccountUpdate(currentAccount);
                                        try
                                        {
                                            bankService.UpdateAccount(bankId, employeeId, updateAccountId, updatedAccount);
                                            consoleMessages.AccountUpdateSuccess();
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
                                            deleteAccountId = bankService.GetAccountIdByUsername(bankId, selectedUsername);
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
                                        string currencyName = consoleUI.GetCurrency();
                                        double exchangeRate = consoleUI.GetExchangeRate();
                                        try
                                        {
                                            bankService.AddCurrency(bankId, currencyName, exchangeRate);
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
                                        string currencyName = consoleUI.GetCurrency();
                                        double exchangeRate = consoleUI.GetExchangeRate();
                                        try
                                        {
                                            bankService.UpdateCurrency(bankId, currencyName, exchangeRate);
                                            consoleMessages.CurrencyUpdateSuccess();
                                        }
                                        catch (CurrencyDoesNotExistException)
                                        {
                                            consoleMessages.CurrencyDoesNotExist();
                                            continue;
                                        }
                                        catch (CurrencyDataInvalidException) { }
                                    }
                                    else if (option3 == AdminOperation.RemoveCurrency)
                                    {
                                        string currencyName = consoleUI.GetCurrency();
                                        try
                                        {
                                            bankService.DeleteCurrency(bankId, currencyName);
                                            consoleMessages.CurrencyDeleteSuccess();
                                        }
                                        catch (CurrencyDoesNotExistException)
                                        {
                                            consoleMessages.CurrencyDoesNotExist();
                                            continue;
                                        }
                                        catch (CurrencyDataInvalidException) { }
                                    }
                                    else if (option3 == AdminOperation.UpdateBank)
                                    {
                                        Bank currentBank = bankService.GetBankDetails(bankId);
                                        Bank updatedBank = consoleUI.GetDataForBankUpdate(currentBank);
                                        try
                                        {
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
                                            selectedAccountId = bankService.GetAccountIdByUsername(bankId, selectedUsername);
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        List<Transaction> transactions = bankService.GetTransactions(bankId, selectedAccountId);
                                        decimal balance = bankService.GetBalance(bankId, selectedAccountId);
                                        consoleUI.PrintTransactions(transactions, balance);
                                    }
                                    else if (option3 == AdminOperation.ActionHistory)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string selectedEmployeeId;
                                        try
                                        {
                                            selectedEmployeeId = bankService.GetEmployeeIdByUsername(bankId, selectedUsername);

                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        List<EmployeeAction> actions = bankService.GetEmployeeActions(bankId, selectedEmployeeId);
                                        consoleUI.PrintEmployeeActions(actions);
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
                                            newAccount = consoleUI.GetDataForAccountCreation();
                                        }
                                        catch (AccountCreationFailedException)
                                        {
                                            consoleMessages.AccountCreationFailed();
                                            continue;
                                        }
                                        try
                                        {
                                            bankService.AddAccount(bankId, employeeId, newAccount);
                                            consoleMessages.AccountCreationSuccess();
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
                                            updateAccountId = bankService.GetAccountIdByUsername(bankId, selecteUsername);
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        currentAccount = bankService.GetAccountDetails(bankId, updateAccountId);
                                        updatedAccount = consoleUI.GetDataForAccountUpdate(currentAccount);
                                        try
                                        {
                                            bankService.UpdateAccount(bankId, employeeId, updateAccountId, updatedAccount);
                                            consoleMessages.AccountUpdateSuccess();
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
                                            deleteAccountId = bankService.GetAccountIdByUsername(bankId, selectedUsername);
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
                                            selectedAccountId = bankService.GetAccountIdByUsername(bankId, selectedUsername);
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        List<Transaction> transactions = bankService.GetTransactions(bankId, selectedAccountId);
                                        decimal balance = bankService.GetBalance(bankId, selectedAccountId);
                                        consoleUI.PrintTransactions(transactions, balance);
                                    }
                                    else if (option4 == StaffOperation.ActionHistory)
                                    {
                                        List<EmployeeAction> actions = bankService.GetEmployeeActions(bankId, employeeId);
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
                                accountId = bankService.GetAccountIdByUsername(bankId, username);
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
                                UserOperation option5 = consoleUI.UserOptions();
                                decimal amount;
                                string txnId;
                                if (option5 == UserOperation.Deposit)
                                {
                                    amount = consoleUI.GetAmount('d');
                                    string currencyName = consoleUI.GetCurrency();
                                    Currency currency = bankService.GetCurrencyByName(bankId, currencyName);
                                    try
                                    {
                                        bankService.Deposit(bankId, accountId, currency, amount);
                                        consoleMessages.DepositSuccess();
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
                                    }
                                    catch (BankDoesnotExistException)
                                    {
                                        consoleMessages.BankDoesnotExistMsg();
                                        continue;
                                    }
                                    string selectedToUsername = consoleUI.GetUsername();
                                    try
                                    {
                                        toAccountId = bankService.GetAccountIdByUsername(toBankId, selectedToUsername);
                                    }
                                    catch (AccountDoesNotExistException)
                                    {
                                        consoleMessages.UserNotFoundMsg();
                                        continue;
                                    }
                                    try
                                    {
                                        bankService.Transfer(bankId, accountId, toBankId, toAccountId, amount);
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
                                else if (option5 == UserOperation.TransactionHistory)
                                {
                                    List<Transaction> transactions = bankService.GetTransactions(bankId, accountId);
                                    decimal balance = bankService.GetBalance(bankId, accountId);
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
