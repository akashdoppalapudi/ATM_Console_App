using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class ConsoleUI
    {
        private ConsoleMessages consoleMessages;
        private BankService bankService;
        private EmployeeService employeeService;
        private AccountService accountService;
        private CurrencyService currencyService;
        public ConsoleUI()
        {
            consoleMessages = new ConsoleMessages();
            bankService = new BankService();
            employeeService = new EmployeeService();
            accountService = new AccountService();
            currencyService = new CurrencyService();
        }

        public (Bank, Employee) GetDataForBankCreation()
        {
            string name;
            Console.WriteLine("\n____BANK CREATION____\n");
            Console.Write("Enter Bank Name : ");
            name = Console.ReadLine();
            if (name.Length < 3)
            {
                Console.WriteLine("Invalid Name");
                throw new BankCreationFailedException();
            }
            bankService.ValidateBankName(name);
            Bank bank = bankService.CreateBank(name);
            string empName, username, password;
            Gender gender;
            Console.WriteLine("\n____ACCOUNT CREATION____\n");
            Console.Write("Please Enter Name : ");
            string selectedName = Console.ReadLine();
            if (selectedName.Length < 3)
            {
                Console.WriteLine("Invalid Name");
                throw new AccountCreationFailedException();
            }
            empName = selectedName;
            Console.WriteLine("\n__GENDER__\n");
            int i = 1;
            foreach (string g in Enum.GetNames(typeof(Gender)))
            {
                Console.WriteLine(i + ". " + g);
                i++;
            }
            Console.Write("\nSelect a Gender : ");
            string selectedGender = Console.ReadLine();
            try
            {
                gender = (Gender)Convert.ToInt32(selectedGender);
                if ((int)gender <= 0 || (int)gender >= i)
                {
                    Console.WriteLine("Invalid Gender");
                    throw new AccountCreationFailedException();
                }
            }
            catch
            {
                Console.WriteLine("Invalid Gender");
                throw new AccountCreationFailedException();
            }
            Console.Write("\nPlease set a Username : ");
            string selectedUsername = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedUsername))
            {
                Console.WriteLine("Invalid Username");
                throw new AccountCreationFailedException();
            }
            username = selectedUsername;
            Console.Write("Please set a Password : ");
            string selectedPassword = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedPassword))
            {
                Console.WriteLine("Invalid Password");
                throw new AccountCreationFailedException();
            }
            password = selectedPassword;
            Employee employee = employeeService.CreateEmployee(empName, gender, username, password, EmployeeType.Admin);
            return (bank, employee);
        }

        public Employee GetDataForEmployeeCreation(string bankId)
        {
            string name, username, password;
            Gender gender;
            EmployeeType employeeType;
            Console.WriteLine("\n____EMPLOYEE CREATION____\n");
            Console.Write("Please Enter Name : ");
            string selectedName = Console.ReadLine();
            if (selectedName.Length < 3)
            {
                Console.WriteLine("Invalid Name");
                throw new AccountCreationFailedException();
            }
            name = selectedName;
            Console.WriteLine("\n__GENDER__\n");
            int i = 1;
            foreach (string g in Enum.GetNames(typeof(Gender)))
            {
                Console.WriteLine(i + ". " + g);
                i++;
            }
            Console.Write("\nSelect a Gender : ");
            string selectedGender = Console.ReadLine();
            try
            {
                gender = (Gender)Convert.ToInt32(selectedGender);
                if ((int)gender <= 0 || (int)gender >= i)
                {
                    Console.WriteLine("Invalid Gender");
                    throw new AccountCreationFailedException();
                }
            }
            catch
            {
                Console.WriteLine("Invalid Gender");
                throw new AccountCreationFailedException();
            }
            Console.Write("\nPlease set a Username : ");
            string selectedUsername = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedUsername))
            {
                throw new AccountCreationFailedException();
            }
            username = selectedUsername;
            employeeService.ValidateUsername(bankId, username);
            Console.Write("Please set a Password : ");
            string selectedPassword = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedPassword))
            {
                Console.WriteLine("Invalid Password");
                throw new AccountCreationFailedException();
            }
            password = selectedPassword;
            Console.WriteLine("\n__EMPLOYEE TYPE__\n");
            i = 1;
            foreach (string type in Enum.GetNames(typeof(EmployeeType)))
            {
                Console.WriteLine(i + ". " + type);
                i++;
            }
            Console.Write("\nSelect an Employee type : ");
            string selectedType = Console.ReadLine();
            try
            {
                employeeType = (EmployeeType)Convert.ToInt32(selectedType);
                if ((int)employeeType <= 0 || (int)employeeType >= i)
                {
                    Console.WriteLine("Invalid Employee Type");
                    throw new AccountCreationFailedException();
                }
            }
            catch
            {
                Console.WriteLine("Invalid Employee Type");
                throw new AccountCreationFailedException();
            }
            return employeeService.CreateEmployee(name, gender, username, password, employeeType);
        }
        public Account GetDataForAccountCreation(string bankId)
        {
            string name, username, password;
            Gender gender;
            AccountType accountType;
            Console.WriteLine("\n____ACCOUNT CREATION____\n");
            Console.Write("Please Enter Name : ");
            string selectedName = Console.ReadLine();
            if (selectedName.Length < 3)
            {
                Console.WriteLine("Invalid Name");
                throw new AccountCreationFailedException();
            }
            name = selectedName;

            Console.WriteLine("\n__GENDER__\n");
            int i = 1;
            foreach (string g in Enum.GetNames(typeof(Gender)))
            {
                Console.WriteLine(i + ". " + g);
                i++;
            }
            Console.Write("\nSelect a Gender : ");
            string selectedGender = Console.ReadLine();
            try
            {
                gender = (Gender)Convert.ToInt32(selectedGender);
                if ((int)gender <= 0 || (int)gender >= i)
                {
                    Console.WriteLine("Invalid Gender");
                    throw new AccountCreationFailedException();
                }
            }
            catch
            {
                Console.WriteLine("Invalid Gender");
                throw new AccountCreationFailedException();
            }
            Console.Write("\nPlease set a Username : ");
            string selectedUsername = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedUsername))
            {
                throw new AccountCreationFailedException();
            }
            username = selectedUsername;
            accountService.ValidateUsername(bankId, username);
            Console.Write("Please set a Password : ");
            string selectedPassword = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedPassword))
            {
                Console.WriteLine("Invalid Password");
                throw new AccountCreationFailedException();
            }
            password = selectedPassword;
            Console.WriteLine("\n__ACCOUNT TYPE__\n");
            i = 1;
            foreach (string type in Enum.GetNames(typeof(AccountType)))
            {
                Console.WriteLine(i + ". " + type);
                i++;
            }
            Console.Write("\nSelect an Account type : ");
            string selectedType = Console.ReadLine();
            try
            {
                accountType = (AccountType)Convert.ToInt32(selectedType);
                if ((int)accountType <= 0 || (int)accountType >= i)
                {
                    Console.WriteLine("Invalid Account Type");
                    throw new AccountCreationFailedException();
                }
            }
            catch
            {
                Console.WriteLine("Invalid Account Type");
                throw new AccountCreationFailedException();
            }
            return accountService.CreateAccount(name, gender, username, password, accountType);
        }

        public Bank GetDataForBankUpdate(Bank currentBank)
        {
            string name;
            double imps, rtgs, oimps, ortgs;
            Console.WriteLine("____BANK UPDATE____");
            Console.Write("[" + currentBank.Name + "] Enter new Bank Name (Leave it empty to not change) : ");
            string userInput = Console.ReadLine();
            if (userInput.Length < 3)
            {
                name = currentBank.Name;
            }
            else
            {
                name = userInput;
                bankService.ValidateBankName(name);
            }
            Console.Write("[" + currentBank.IMPS + "] Enter new IMPS for same bank transfer (Leave it empty to not change) : ");
            userInput = Console.ReadLine();
            if (String.IsNullOrEmpty(userInput))
            {
                imps = currentBank.IMPS;
            }
            else
            {
                try
                {
                    imps = Convert.ToDouble(userInput);
                    if (imps < 0)
                    {
                        Console.WriteLine("Invalid Input! Keeping the previous IMPS");
                        imps = currentBank.IMPS;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Input! Keeping the previous IMPS");
                    imps = currentBank.IMPS;
                }
            }
            Console.Write("[" + currentBank.RTGS + "] Enter new RTGS for same bank transfer (Leave it empty to not change) : ");
            userInput = Console.ReadLine();
            if (String.IsNullOrEmpty(userInput))
            {
                rtgs = currentBank.RTGS;
            }
            else
            {
                try
                {
                    rtgs = Convert.ToDouble(userInput);
                    if (rtgs < 0)
                    {
                        Console.WriteLine("Invalid Input! Keeping the previous RTGS");
                        imps = currentBank.RTGS;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Input! Keeping the previous RTGS");
                    rtgs = currentBank.RTGS;
                }
            }
            Console.Write("[" + currentBank.OIMPS + "] Enter new IMPS for same other bank transfer (Leave it empty to not change) : ");
            userInput = Console.ReadLine();
            if (String.IsNullOrEmpty(userInput))
            {
                oimps = currentBank.OIMPS;
            }
            else
            {
                try
                {
                    oimps = Convert.ToDouble(userInput);
                    if (oimps < 0)
                    {
                        Console.WriteLine("Invalid Input! Keeping the previous OIMPS");
                        imps = currentBank.IMPS;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Input! Keeping the previous OIMPS");
                    oimps = currentBank.OIMPS;
                }
            }
            Console.Write("[" + currentBank.ORTGS + "] Enter new RTGS for other bank transfer (Leave it empty to not change) : ");
            userInput = Console.ReadLine();
            if (String.IsNullOrEmpty(userInput))
            {
                ortgs = currentBank.ORTGS;
            }
            else
            {
                try
                {
                    ortgs = Convert.ToDouble(userInput);
                    if (ortgs < 0)
                    {
                        Console.WriteLine("Invalid Input! Keeping the previous ORTGS");
                        imps = currentBank.ORTGS;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Input! Keeping the previous RTGS");
                    ortgs = currentBank.ORTGS;
                }
            }

            return new Bank { Name = name, IMPS = imps, RTGS = rtgs, OIMPS = oimps, ORTGS = ortgs };
        }

        public Employee GetDataForEmployeeUpdate(Employee currentEmployee)
        {
            string name, username, password;
            Gender gender;
            EmployeeType employeeType;
            Console.WriteLine("\n____EMPLOYEE UPDATE____\n");
            Console.Write("[" + currentEmployee.Name + "] Please Enter a new Name (Leave it empty to not change) : ");
            string selectedName = Console.ReadLine();
            if (selectedName.Length < 3)
            {
                name = currentEmployee.Name;
            }
            else
            {
                name = selectedName;
            }
            Console.WriteLine("\n__GENDER__\n");
            int i = 1;
            foreach (string g in Enum.GetNames(typeof(Gender)))
            {
                Console.WriteLine(i + ". " + g);
                i++;
            }
            Console.Write("\n[" + Enum.GetName(typeof(Gender), currentEmployee.Gender) + "] Select a Gender (Leave it empty to not change) : ");
            string selectedGender = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedGender))
            {
                gender = currentEmployee.Gender;
            }
            else
            {
                try
                {
                    gender = (Gender)Convert.ToInt32(selectedGender);
                    if ((int)gender <= 0 || (int)gender >= i)
                    {
                        Console.WriteLine("Invalid Gender. Keeping the previous gender");
                        gender = currentEmployee.Gender;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Gender. Keeping the previous gender");
                    gender = currentEmployee.Gender;
                }
            }
            Console.Write("\n[" + currentEmployee.Username + "] Please set a new Username (Leave it empty to not change) : ");
            string selectedUsername = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedUsername))
            {
                username = currentEmployee.Username;
            }
            else
            {
                username = selectedUsername;
                employeeService.ValidateUsername(currentEmployee.BankId, username);
            }
            Console.Write("Please set a new Password (Leave it empty to not change) : ");
            password = Console.ReadLine();

            Console.WriteLine("\n__EMPLOYEE TYPE__\n");
            i = 1;
            foreach (string type in Enum.GetNames(typeof(EmployeeType)))
            {
                Console.WriteLine(i + ". " + type);
                i++;
            }
            Console.Write("\n[" + Enum.GetName(typeof(EmployeeType), currentEmployee.EmployeeType) + "]Select an Employee type (Leave it empty to not change) : ");
            string selectedType = Console.ReadLine();
            if (selectedType == null)
            {
                employeeType = currentEmployee.EmployeeType;
            }
            else
            {
                try
                {
                    employeeType = (EmployeeType)Convert.ToInt32(selectedType);
                    if ((int)employeeType <= 0 || (int)employeeType >= i)
                    {
                        Console.WriteLine("Invalid Employee Type. Keeping previous Employee Type");
                        employeeType = currentEmployee.EmployeeType;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Employee Type. Keeping Previous EmployeeType");
                    employeeType = currentEmployee.EmployeeType;
                }
            }

            return employeeService.CreateEmployee(name, gender, username, password, employeeType);
        }

        public Account GetDataForAccountUpdate(Account currentAccount)
        {
            string name, username, password;
            Gender gender;
            AccountType accountType;
            Console.WriteLine("\n____ACCOUNT UPDATE____\n");
            Console.Write("[" + currentAccount.Name + "] Please Enter a new Name (Leave it empty to not change) : ");
            string selectedName = Console.ReadLine();
            if (selectedName.Length < 3)
            {
                name = currentAccount.Name;
            }
            else
            {
                name = selectedName;
            }

            Console.WriteLine("\n__GENDER__\n");
            int i = 1;
            foreach (string g in Enum.GetNames(typeof(Gender)))
            {
                Console.WriteLine(i + ". " + g);
                i++;
            }
            Console.Write("\n[" + Enum.GetName(typeof(Gender), currentAccount.Gender) + "] Select a Gender (Leave it empty to not change) : ");
            string selectedGender = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedGender))
            {
                gender = currentAccount.Gender;
            }
            else
            {
                try
                {
                    gender = (Gender)Convert.ToInt32(selectedGender);
                    if ((int)gender <= 0 || (int)gender >= i)
                    {
                        Console.WriteLine("Invalid Gender. Keeping the previous gender");
                        gender = currentAccount.Gender;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Gender. Keeping the previous gender");
                    gender = currentAccount.Gender;
                }
            }

            Console.Write("\n[" + currentAccount.Username + "] Please set a new Username (Leave it empty to not change) : ");
            string selectedUsername = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedUsername))
            {
                username = currentAccount.Username;
            }
            else
            {
                username = selectedUsername;
                accountService.ValidateUsername(currentAccount.BankId, username);
            }

            Console.Write("Please set a new Password (Leave it empty to not change) : ");
            password = Console.ReadLine();

            Console.WriteLine("\n__Account TYPE__\n");
            i = 1;
            foreach (string type in Enum.GetNames(typeof(AccountType)))
            {
                Console.WriteLine(i + ". " + type);
                i++;
            }
            Console.Write("\n[" + Enum.GetName(typeof(AccountType), currentAccount.AccountType) + "]Select an Employee type (Leave it empty to not change) : ");
            string selectedType = Console.ReadLine();
            if (selectedType == null)
            {
                accountType = currentAccount.AccountType;
            }
            else
            {
                try
                {
                    accountType = (AccountType)Convert.ToInt32(selectedType);
                    if ((int)accountType <= 0 || (int)accountType >= i)
                    {
                        Console.WriteLine("Invalid Employee Type. Keeping previous Employee Type");
                        accountType = currentAccount.AccountType;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Employee Type. Keeping Previous EmployeeType");
                    accountType = currentAccount.AccountType;
                }
            }

            return accountService.CreateAccount(name, gender, username, password, accountType);
        }

        public string SelectBank(Dictionary<string, string> bankNames)
        {
            Console.WriteLine("\n____BANKS____\n");
            int i = 1;
            foreach (string name in bankNames.Values)
            {
                Console.WriteLine(i + ". " + name);
                i++;
            }
            Console.Write("\nSelect a Bank : ");
            string userInput = Console.ReadLine();
            try
            {
                int selectedOption = Convert.ToInt32(userInput);
                if (selectedOption >= 1 && selectedOption <= bankNames.Count)
                {
                    return bankNames.ElementAt(selectedOption - 1).Key;
                }
                else
                {
                    consoleMessages.InvalidOptionMsg();
                    return null;
                }
            }
            catch
            {
                consoleMessages.InvalidOptionMsg();
                return null;
            }
        }

        public decimal GetAmount(char amtFor)
        {
            decimal amount;
            switch (amtFor)
            {
                case 'd':
                    Console.WriteLine("\n____DEPOSIT____\n");
                    Console.Write("Enter Amount to be deposited : ");
                    break;
                case 'w':
                    Console.WriteLine("\n____WITHDRAW____\n");
                    Console.Write("Enter Amount to be withdrawn : ");
                    break;
                case 't':
                    Console.WriteLine("\n\n____TRANSFER____\n");
                    Console.Write("Enter Amount to be transferred : ");
                    break;
            }
            string userInput = Console.ReadLine();
            try
            {
                amount = decimal.Parse(userInput);
            }
            catch
            {
                amount = -1;
            }
            return amount;
        }

        public string GetUsername()
        {
            Console.Write("\nEnter account's Username : ");
            string username = Console.ReadLine();
            return username;
        }

        public string GetCurrencyName()
        {
            Console.Write("\nEnter Currency Name : ");
            string currencyName = Console.ReadLine();
            return currencyName;
        }

        public string GetRevertTransactionId()
        {
            string txnId;
            Console.WriteLine("\n____REVERT TRANSACTION____\n");
            Console.Write("Enter the transaction id : ");
            txnId = Console.ReadLine();
            return txnId;
        }

        public Currency GetDataForCurrencyCreation(string bankId)
        {
            string currencyName;
            double exchangeRate;
            Console.Write("Enter Currency Name : ");
            string userInput = Console.ReadLine();
            if (userInput == null || userInput.Length != 3)
            {
                Console.WriteLine("Invalid Name");
                throw new CurrencyDataInvalidException();
            }
            currencyService.ValidateCurrencyName(bankId, userInput);
            currencyName = userInput;
            Console.Write("Enter Exchange Rate : ");
            userInput = Console.ReadLine();
            if (String.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Invalid Exchange Rate");
                throw new CurrencyDataInvalidException();
            }
            try
            {
                exchangeRate = Convert.ToDouble(userInput);
                if (exchangeRate <= 0)
                {
                    Console.WriteLine("Invalid Exchange Rate");
                    throw new CurrencyDataInvalidException();
                }
            }
            catch
            {
                Console.WriteLine("Invalid Exchange Rate");
                throw new CurrencyDataInvalidException();
            }
            return currencyService.CreateCurrency(currencyName, exchangeRate);
        }

        public Currency GetDataForCurrencyUpdate(Currency currentCurrency)
        {
            Console.Write("[" + currentCurrency.ExchangeRate + "] Enter new Exchange rate for '" + currentCurrency.Name + "' (Leave it Empty to not change) : ");
            string userInput = Console.ReadLine();
            double exchangeRate;
            if (String.IsNullOrEmpty(userInput))
            {
                exchangeRate = currentCurrency.ExchangeRate;
            }
            else
            {
                try
                {
                    exchangeRate = Convert.ToDouble(userInput);
                    if (exchangeRate < 0)
                    {
                        Console.WriteLine("Invalid Input! Keeping the previous Exchange Rate");
                        exchangeRate = currentCurrency.ExchangeRate;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Input! Keeping the previous Exchange Rate");
                    exchangeRate = currentCurrency.ExchangeRate;
                }
            }
            return currencyService.CreateCurrency(currentCurrency.Name, exchangeRate);
        }

        public string GetPasswordFromUser()
        {
            Console.WriteLine("\n____AUTHENTICATION____\n");
            Console.Write("Enter Password : ");
            string userInput = Console.ReadLine();
            return userInput;
        }

        public void PrintTransactions(IList<Transaction> transactions, decimal availBal)
        {
            Console.WriteLine("\n____TRANSACTION HISTORY____\n");
            Console.WriteLine("Date\tTXN ID\tDebit/Credit\tFrom\tTo\tNarrative\tAmount\n");
            foreach (Transaction transaction in transactions)
            {
                Console.WriteLine(transaction.TransactionDate + "\t" + transaction.Id + "\t" + transaction.TransactionType + "\t" + transaction.AccountId + "\t" + transaction.ToAccountId + "\t" + transaction.TransactionNarrative + "\tRs. " + transaction.TransactionAmount);
            }
            Console.WriteLine("\t\t\t\tAvailable Balance : " + availBal);
        }

        public void PrintEmployeeActions(IList<EmployeeAction> employeeActions)
        {
            Console.WriteLine("\n____ACTION HISTORY____\n");
            Console.WriteLine("Date\tACN ID\tAction Type\tAccount ID\tTXN ID\n");
            foreach (EmployeeAction action in employeeActions)
            {
                Console.WriteLine(action.ActionDate + "\t" + action.Id + "\t" + action.ActionType + "\t" + action.AccountId + "\n" + action.TXNId);
            }
        }

        public BankCreateOrSelect SelectOrCreateBank()
        {
            BankCreateOrSelect option;
            Console.WriteLine("\n____OPTIONS____\n");
            Console.WriteLine("1. Create New Bank\n2. Select a Bank\n3. Exit");
            Console.Write("\nSelect an Option : ");
            string userInput = Console.ReadLine();
            try
            {
                option = (BankCreateOrSelect)Convert.ToInt32(userInput);
            }
            catch
            {
                option = (BankCreateOrSelect)0;
            }
            return option;
        }

        public StaffOrUserLogin UserOrStaff()
        {
            StaffOrUserLogin option;
            Console.WriteLine("\n____OPTIONS____\n");
            Console.WriteLine("1. Login as Staff\n2. Login as User\n3. Back");
            Console.Write("\nSelect an Option : ");
            string userInput = Console.ReadLine();
            try
            {
                option = (StaffOrUserLogin)(Convert.ToInt32(userInput));
            }
            catch
            {
                option = (StaffOrUserLogin)0;
            }
            return option;
        }

        public AdminOperation AdminOptions()
        {
            AdminOperation operation;
            Console.WriteLine("\n____OPERATIONS____\n");
            Console.WriteLine("\n1. Create a new Employee\n2. Update an Employee\n3. Delete an Employee\n4. Create a new Account\n5. Update an Account\n6. Delete an Account\n7. Add Currency\n8. Change Currency\n9. Remove Currency\n10. Update Bank\n11. Delete Bank\n12. Revert Transaction\n13. View Transaction History\n14. View Action History\n15. Back");
            Console.Write("\nSelect an operation : ");
            string userInput = Console.ReadLine();
            try
            {
                operation = (AdminOperation)(Convert.ToInt32(userInput));
            }
            catch
            {
                operation = (AdminOperation)0;
            }
            return operation;
        }

        public StaffOperation StaffOptions()
        {
            StaffOperation operation;
            Console.WriteLine("\n____OPERATIONS____\n");
            Console.WriteLine("\n1. Create a new Account\n2. Update an Account\n3. Delete an Account\n4. Revert Transaction\n5. View Transaction History\n6. View Action History\n7. Back");
            Console.Write("\nSelect an operation : ");
            string userInput = Console.ReadLine();
            try
            {
                operation = (StaffOperation)(Convert.ToInt32(userInput));
            }
            catch
            {
                operation = (StaffOperation)0;
            }
            return operation;
        }

        public UserOperation UserOptions()
        {
            UserOperation operation;
            Console.WriteLine("\n____OPERATIONS____\n");
            Console.WriteLine("\n1. Deposit\n2. Withdraw\n3. Transfer\n4. View Transaction History\n5. Back");
            Console.Write("\nSelect an operation : ");
            string userInput = Console.ReadLine();
            try
            {
                operation = (UserOperation)(Convert.ToInt32(userInput));
            }
            catch
            {
                operation = (UserOperation)0;
            }
            return operation;
        }
    }
}
