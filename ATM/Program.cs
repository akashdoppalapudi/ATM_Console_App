using ATM.Models;
using ATM.Models.Enums;
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
                        if (option2 == Option2.StaffLogin)
                        {
                            string username, password, employeeId;
                            Tuple<string, EmployeeType> employee;
                            username = consoleUI.GetUsername();
                            try
                            {
                                employee = bankService.CheckEmployeeExistance(bankId, username);
                                employeeId = employee.Item1;
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
                            if (employee.Item2 == EmployeeType.Admin)
                            {
                                while (true)
                                {
                                    Option3 option3 = consoleUI.AdminOptions();
                                    if (option3 == Option3.CreateEmployee)
                                    {
                                        Tuple<string, Gender, string, string, EmployeeType> employeeDetails = consoleUI.GetDataForEmployeeCreation();
                                        try
                                        {
                                            Tuple<string, EmployeeType> newEmployee = bankService.CreateNewEmployee(bankId, employeeId, employeeDetails);
                                            consoleMessages.AccountCreationSuccess();
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
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
                                    else if (option3 == Option3.UpdateEmployee)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string updateEmployeeId;
                                        Tuple<string, Gender, string, EmployeeType> currentEmployeeDetails;
                                        Tuple<string, Gender, string, string, EmployeeType> updatedEmployeeDetails;
                                        try
                                        {
                                            Tuple<string, EmployeeType> updateEmployee = bankService.CheckEmployeeExistance(bankId, selectedUsername);
                                            updateEmployeeId = updateEmployee.Item1;
                                        }
                                        catch (EmployeeDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        currentEmployeeDetails = bankService.GetEmployeeDetails(bankId, updateEmployeeId);
                                        updatedEmployeeDetails = consoleUI.GetDataForEmployeeUpdate(currentEmployeeDetails);
                                        try
                                        {
                                            string updatedEmployeeId = bankService.UpdateEmployee(bankId, employeeId, updateEmployeeId, updatedEmployeeDetails);
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
                                    else if (option3 == Option3.DeleteEmployee)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string deleteEmployeeId;
                                        try
                                        {
                                            Tuple<string, EmployeeType> deleteEmployee = bankService.CheckEmployeeExistance(bankId, selectedUsername);
                                            deleteEmployeeId = deleteEmployee.Item1;
                                        }
                                        catch (EmployeeDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        try
                                        {
                                            deleteEmployeeId = bankService.DeleteEmployee(bankId, employeeId, deleteEmployeeId);
                                            consoleMessages.EmployeeDeleteSuccess();
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
                                        }
                                    }
                                    else if (option3 == Option3.CreateAccount)
                                    {
                                        Tuple<string, Gender, string, string, AccountType> accountDetails = consoleUI.GetDataForAccountCreation();
                                        try
                                        {
                                            string newAccountId = bankService.CreateNewAccount(bankId, employeeId, accountDetails);
                                            consoleMessages.AccountCreationSuccess();
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
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
                                    else if (option3 == Option3.UpdateAccount)
                                    {
                                        string selecteUsername = consoleUI.GetUsername();
                                        string updateAccountId;
                                        Tuple<string, Gender, string, AccountType> currentAccountDetails;
                                        Tuple<string, Gender, string, string, AccountType> updatedAccountDetails;
                                        try
                                        {
                                            updateAccountId = bankService.CheckAccountExistance(bankId, selecteUsername);
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        currentAccountDetails = bankService.GetAccountDetails(bankId, updateAccountId);
                                        updatedAccountDetails = consoleUI.GetDataForAccountUpdate(currentAccountDetails);
                                        try
                                        {
                                            string updatedAccountId = bankService.UpdateAccount(bankId, employeeId, updateAccountId, updatedAccountDetails);
                                            consoleMessages.AccountUpdateSuccess();
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
                                    else if (option3 == Option3.DeleteAccount)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string deleteAccountId;
                                        try
                                        {
                                            deleteAccountId = bankService.CheckAccountExistance(bankId, selectedUsername);
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        try
                                        {
                                            deleteAccountId = bankService.DeleteAccount(bankId, employeeId, deleteAccountId);
                                            consoleMessages.AccountDeleteSuccess();
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
                                        }
                                    }
                                    else if (option3 == Option3.AddCurrency)
                                    {

                                    }
                                    else if (option3 == Option3.UpdateBank)
                                    {
                                        Tuple<string> bankDetails = bankService.GetBankDetails(bankId);
                                        Tuple<string> updateBankDetails = consoleUI.GetDataForBankUpdate(bankDetails);
                                        try
                                        {
                                            string updatebankId = bankService.UpdateBank(bankId, employeeId, updateBankDetails);
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
                                    else if (option3 == Option3.DeleteBank)
                                    {
                                        try
                                        {
                                            string deleteBankId = bankService.DeleteBank(bankId, employeeId);
                                            consoleMessages.BankDeleteSuccess();
                                            goto EndOfProgram;
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
                                        }
                                    }
                                    else if (option3 == Option3.TransactionHistory)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string selectedAccountId;
                                        try
                                        {
                                            selectedAccountId = bankService.CheckAccountExistance(bankId, selectedUsername);
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
                                    else if (option3 == Option3.ActionHistory)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string selectedEmployeeId;
                                        try
                                        {
                                            Tuple<string, EmployeeType> selectedEmployee = bankService.CheckEmployeeExistance(bankId, selectedUsername);
                                            selectedEmployeeId = selectedEmployee.Item1;
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        List<EmployeeAction> actions = bankService.GetEmployeeActions(bankId, selectedEmployeeId);
                                        consoleUI.PrintEmployeeActions(actions);
                                    }
                                    else if (option3 == Option3.Back)
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
                                    Option4 option4 = consoleUI.StaffOptions();
                                    if (option4 == Option4.CreateAccount)
                                    {
                                        Tuple<string, Gender, string, string, AccountType> accountDetails = consoleUI.GetDataForAccountCreation();
                                        try
                                        {
                                            string newAccountId = bankService.CreateNewAccount(bankId, employeeId, accountDetails);
                                            consoleMessages.AccountCreationSuccess();
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
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
                                    else if (option4 == Option4.UpdateAccount)
                                    {
                                        string selecteUsername = consoleUI.GetUsername();
                                        string updateAccountId;
                                        Tuple<string, Gender, string, AccountType> currentAccountDetails;
                                        Tuple<string, Gender, string, string, AccountType> updatedAccountDetails;
                                        try
                                        {
                                            updateAccountId = bankService.CheckAccountExistance(bankId, selecteUsername);
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        currentAccountDetails = bankService.GetAccountDetails(bankId, updateAccountId);
                                        updatedAccountDetails = consoleUI.GetDataForAccountUpdate(currentAccountDetails);
                                        try
                                        {
                                            string updatedAccountId = bankService.UpdateAccount(bankId, employeeId, updateAccountId, updatedAccountDetails);
                                            consoleMessages.AccountUpdateSuccess();
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
                                    else if (option4 == Option4.DeleteAccount)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string deleteAccountId;
                                        try
                                        {
                                            deleteAccountId = bankService.CheckAccountExistance(bankId, selectedUsername);
                                        }
                                        catch (AccountDoesNotExistException)
                                        {
                                            consoleMessages.UserNotFoundMsg();
                                            continue;
                                        }
                                        try
                                        {
                                            deleteAccountId = bankService.DeleteAccount(bankId, employeeId, deleteAccountId);
                                            consoleMessages.AccountDeleteSuccess();
                                        }
                                        catch (AccessDeniedException)
                                        {
                                            consoleMessages.AccessDeniedMsg();
                                            continue;
                                        }
                                    }
                                    else if (option4 == Option4.TransactionHistory)
                                    {
                                        string selectedUsername = consoleUI.GetUsername();
                                        string selectedAccountId;
                                        try
                                        {
                                            selectedAccountId = bankService.CheckAccountExistance(bankId, selectedUsername);
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
                                    else if (option4 == Option4.ActionHistory)
                                    {
                                        List<EmployeeAction> actions = bankService.GetEmployeeActions(bankId, employeeId);
                                        consoleUI.PrintEmployeeActions(actions);
                                    }
                                    else if (option4 == Option4.Back)
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
                                    catch (BankDoesnotExistException)
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
                                    consoleUI.PrintTransactions(transactions, balance);
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
            EndOfProgram:;
            }
        }
    }
}
