using ATM.Models;
using ATM.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class ConsoleUI
    {
        ConsoleMessages consoleMessages = new ConsoleMessages();

        public (Tuple<string>, Tuple<string, Gender, string, string>) GetDataForBankCreation()
        {
            string name;
            Console.WriteLine("\n____BANK CREATION____\n");
            Console.Write("Enter Bank Name : ");
            name = Console.ReadLine();
            Tuple<string> bankDetails = Tuple.Create(name);
            string empName, username, password;
            Gender gender;
            Console.WriteLine("\n____ACCOUNT CREATION____\n");
            Console.Write("Please Enter Name : ");
            string selectedName = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedName))
            {
                Console.WriteLine("Invalid Name");
                return (null, null);
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
                    return (null, null);
                }
            }
            catch
            {
                Console.WriteLine("Invalid Gender");
                return (null, null);
            }
            Console.Write("\nPlease set a Username : ");
            string selectedUsername = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedUsername))
            {
                return (null, null);
            }
            username = selectedUsername;
            Console.Write("Please set a Password : ");
            string selectedPassword = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedPassword))
            {
                Console.WriteLine("Invalid Password");
                return (null, null);
            }
            password = selectedPassword;
            Tuple<string, Gender, string, string> employeeDetails = Tuple.Create(empName, gender, username, password);
            return (bankDetails, employeeDetails);
        }

        public Tuple<string, Gender, string, string, EmployeeType> GetDataForEmployeeCreation()
        {
            string name, username, password;
            Gender gender;
            EmployeeType employeeType;
            Console.WriteLine("\n____EMPLOYEE CREATION____\n");
            Console.Write("Please Enter Name : ");
            string selectedName = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedName))
            {
                Console.WriteLine("Invalid Name");
                return null;
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
                    return null;
                }
            }
            catch
            {
                Console.WriteLine("Invalid Gender");
                return null;
            }

            Console.Write("\nPlease set a Username : ");
            string selectedUsername = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedUsername))
            {
                return null;
            }
            username = selectedUsername;

            Console.Write("Please set a Password : ");
            string selectedPassword = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedPassword))
            {
                Console.WriteLine("Invalid Password");
                return null;
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
                    return null;
                }
            }
            catch
            {
                Console.WriteLine("Invalid Employee Type");
                return null;
            }

            return Tuple.Create(name, gender, username, password, employeeType);
        }
        public Tuple<string, Gender, string, string, AccountType> GetDataForAccountCreation()
        {
            string name, username, password;
            Gender gender;
            AccountType accountType;
            Console.WriteLine("\n____ACCOUNT CREATION____\n");
            Console.Write("Please Enter Name : ");
            string selectedName = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedName))
            {
                Console.WriteLine("Invalid Name");
                return null;
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
                    return null;
                }
            }
            catch
            {
                Console.WriteLine("Invalid Gender");
                return null;
            }

            Console.Write("\nPlease set a Username : ");
            string selectedUsername = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedUsername))
            {
                return null;
            }
            username = selectedUsername;

            Console.Write("Please set a Password : ");
            string selectedPassword = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedPassword))
            {
                Console.WriteLine("Invalid Password");
                return null;
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
                    return null;
                }
            }
            catch
            {
                Console.WriteLine("Invalid Account Type");
                return null;
            }

            return Tuple.Create(name, gender, username, password, accountType);
        }

        public Tuple<string> GetDataForBankUpdate(Tuple<string> bankDetails)
        {
            string name;
            Console.WriteLine("____BANK UPDATE____");
            Console.Write("[" + bankDetails.Item1 + "] Enter new Bank Name (Leave it empty to not change) : ");
            string userInput = Console.ReadLine();
            if (String.IsNullOrEmpty(userInput))
            {
                name = bankDetails.Item1;
            }
            else
            {
                name = userInput;
            }
            return Tuple.Create(name);
        }

        public Tuple<string, Gender, string, string, EmployeeType> GetDataForEmployeeUpdate(Tuple<string, Gender, string, EmployeeType> employeeDetails)
        {
            string name, username, password;
            Gender gender;
            EmployeeType employeeType;
            Console.WriteLine("\n____EMPLOYEE UPDATE____\n");
            Console.Write("[" + employeeDetails.Item1 + "] Please Enter a new Name (Leave it empty to not change) : ");
            string selectedName = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedName))
            {
                name = employeeDetails.Item1;
            }
            name = selectedName;

            Console.WriteLine("\n__GENDER__\n");
            int i = 1;
            foreach (string g in Enum.GetNames(typeof(Gender)))
            {
                Console.WriteLine(i + ". " + g);
                i++;
            }
            Console.Write("\n[" + Enum.GetName(typeof(Gender), employeeDetails.Item2) + "] Select a Gender (Leave it empty to not change) : ");
            string selectedGender = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedGender))
            {
                gender = employeeDetails.Item2;
            }
            else
            {
                try
                {
                    gender = (Gender)Convert.ToInt32(selectedGender);
                    if ((int)gender <= 0 || (int)gender >= i)
                    {
                        Console.WriteLine("Invalid Gender. Keeping the previous gender");
                        gender = employeeDetails.Item2;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Gender. Keeping the previous gender");
                    gender = employeeDetails.Item2;
                }
            }

            Console.Write("\n[" + employeeDetails.Item3 + "] Please set a new Username (Leave it empty to not change) : ");
            string selectedUsername = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedUsername))
            {
                username = employeeDetails.Item3;
            }
            else
            {
                username = selectedUsername;
            }

            Console.Write("Please set a new Password (Leave it empty to not change) : ");
            string selectedPassword = Console.ReadLine();
            
            if (String.IsNullOrEmpty(selectedPassword))
            {
                password = null;
            }
            password = selectedPassword;

            Console.WriteLine("\n__EMPLOYEE TYPE__\n");
            i = 1;
            foreach (string type in Enum.GetNames(typeof(EmployeeType)))
            {
                Console.WriteLine(i + ". " + type);
                i++;
            }
            Console.Write("\n[" + Enum.GetName(typeof(EmployeeType), employeeDetails.Item4) + "]Select an Employee type (Leave it empty to not change) : ");
            string selectedType = Console.ReadLine();
            if (selectedType == null)
            {
                employeeType = employeeDetails.Item4;
            }
            else
            {
                try
                {
                    employeeType = (EmployeeType)Convert.ToInt32(selectedType);
                    if ((int)employeeType <= 0 || (int)employeeType >= i)
                    {
                        Console.WriteLine("Invalid Employee Type. Keeping previous Employee Type");
                        employeeType = employeeDetails.Item4;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Employee Type. Keeping Previous EmployeeType");
                    employeeType = employeeDetails.Item4;
                }
            }

            return Tuple.Create(name, gender, username, password, employeeType);
        }

        public Tuple<string, Gender, string, string, AccountType> GetDataForAccountUpdate(Tuple<string, Gender, string, AccountType> accountDetails)
        {
            string name, username, password;
            Gender gender;
            AccountType accountType;
            Console.WriteLine("\n____ACCOUNT UPDATE____\n");
            Console.Write("[" + accountDetails.Item1 + "] Please Enter a new Name (Leave it empty to not change) : ");
            string selectedName = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedName))
            {
                name = accountDetails.Item1;
            }
            name = selectedName;

            Console.WriteLine("\n__GENDER__\n");
            int i = 1;
            foreach (string g in Enum.GetNames(typeof(Gender)))
            {
                Console.WriteLine(i + ". " + g);
                i++;
            }
            Console.Write("\n[" + Enum.GetName(typeof(Gender), accountDetails.Item2) + "] Select a Gender (Leave it empty to not change) : ");
            string selectedGender = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedGender))
            {
                gender = accountDetails.Item2;
            }
            else
            {
                try
                {
                    gender = (Gender)Convert.ToInt32(selectedGender);
                    if ((int)gender <= 0 || (int)gender >= i)
                    {
                        Console.WriteLine("Invalid Gender. Keeping the previous gender");
                        gender = accountDetails.Item2;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Gender. Keeping the previous gender");
                    gender = accountDetails.Item2;
                }
            }

            Console.Write("\n[" + accountDetails.Item3 + "] Please set a new Username (Leave it empty to not change) : ");
            string selectedUsername = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedUsername))
            {
                username = accountDetails.Item3;
            }
            else
            {
                username = selectedUsername;
            }

            Console.Write("Please set a new Password (Leave it empty to not change) : ");
            string selectedPassword = Console.ReadLine();
            if (String.IsNullOrEmpty(selectedPassword))
            {
                password = null;
            }
            password = selectedPassword;

            Console.WriteLine("\n__Account TYPE__\n");
            i = 1;
            foreach (string type in Enum.GetNames(typeof(AccountType)))
            {
                Console.WriteLine(i + ". " + type);
                i++;
            }
            Console.Write("\n[" + Enum.GetName(typeof(AccountType), accountDetails.Item4) + "]Select an Employee type (Leave it empty to not change) : ");
            string selectedType = Console.ReadLine();
            if (selectedType == null)
            {
                accountType = accountDetails.Item4;
            }
            else
            {
                try
                {
                    accountType = (AccountType)Convert.ToInt32(selectedType);
                    if ((int)accountType <= 0 || (int)accountType >= i)
                    {
                        Console.WriteLine("Invalid Employee Type. Keeping previous Employee Type");
                        accountType = accountDetails.Item4;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Employee Type. Keeping Previous EmployeeType");
                    accountType = accountDetails.Item4;
                }
            }

            return Tuple.Create(name, gender, username, password, accountType);
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

        public string GetTransferToUsername()
        {
            Console.Write("\nEnter Reciever's Username : ");
            string username = Console.ReadLine();
            return username;
        }

        public string GetPasswordFromUser()
        {
            Console.WriteLine("\n____AUTHENTICATION____\n");
            Console.Write("Enter Password : ");
            string userInput = Console.ReadLine();
            return userInput;
        }

        public void PrintTransactions(List<Transaction> transactions, decimal availBal)
        {
            Console.WriteLine("\n____TRANSACTION HISTORY____\n");
            Console.WriteLine("Date\tTXN ID\tDebit/Credit\tFrom\tTo\tNarrative\tAmount\n");
            foreach (Transaction transaction in transactions)
            {
                Console.WriteLine(transaction.TransactionDate + "\t" + transaction.Id + "\t" + transaction.TransactionType + "\t" + transaction.FromAccountId + "\t" + transaction.ToAccountId + "\t" + transaction.TransactionNarrative + "\tRs. " + transaction.TransactionAmount);
            }
            Console.WriteLine("\t\t\t\tAvailable Balance : " + availBal);
        }

        public void PrintEmployeeActions(List<EmployeeAction> employeeActions)
        {
            Console.WriteLine("\n____ACTION HISTORY____\n");
            Console.WriteLine("Date\tACN ID\tAction Type\tAccount ID\tTXN ID\n");
            foreach (EmployeeAction action in employeeActions)
            {
                Console.WriteLine(action.ActionDate + "\t" + action.Id + "\t" + action.ActionType + "\t" + action.AccountId + "\n" + action.TXNId);
            }
        }

        public Option1 SelectOrCreateBank()
        {
            Option1 option;
            Console.WriteLine("\n____OPTIONS____\n");
            Console.WriteLine("1. Create New Bank\n2. Select a Bank\n3. Exit");
            Console.Write("\nSelect an Option : ");
            string userInput = Console.ReadLine();
            try
            {
                option = (Option1)Convert.ToInt32(userInput);
            }
            catch
            {
                option = (Option1)0;
            }
            return option;
        }

        public Option2 UserOrStaff()
        {
            Option2 option;
            Console.WriteLine("\n____OPTIONS____\n");
            Console.WriteLine("1. Login as Staff\n2. Login as User\n3. Back");
            Console.Write("\nSelect an Option : ");
            string userInput = Console.ReadLine();
            try
            {
                option = (Option2)(Convert.ToInt32(userInput));
            }
            catch
            {
                option = (Option2)0;
            }
            return option;
        }

        public Option3 AdminOptions()
        {
            Option3 operation;
            Console.WriteLine("\n____OPERATIONS____\n");
            Console.WriteLine("\n1. Create a new Employee\n2. Update an Employee\n3. Delete an Employee\n4. Create a new Account\n5. Update an Account\n6. Delete an Account\n7. Add Currency\n8. Update Bank\n9. Delete Bank\n10. View Transaction History\n11. View Action History\n12. Back");
            Console.Write("\nSelect an operation : ");
            string userInput = Console.ReadLine();
            try
            {
                operation = (Option3)(Convert.ToInt32(userInput));
            }
            catch
            {
                operation = (Option3)0;
            }
            return operation;
        }

        public Option4 StaffOptions()
        {
            Option4 operation;
            Console.WriteLine("\n____OPERATIONS____\n");
            Console.WriteLine("\n1. Create a new Account\n2. Update an Account\n3. Delete an Account\n4. View Transaction History\n5. View Action History\n6. Back");
            Console.Write("\nSelect an operation : ");
            string userInput = Console.ReadLine();
            try
            {
                operation = (Option4)(Convert.ToInt32(userInput));
            }
            catch
            {
                operation = (Option4)0;
            }
            return operation;
        }

        public Option5 UserOptions()
        {
            Option5 operation;
            Console.WriteLine("\n____OPERATIONS____\n");
            Console.WriteLine("\n1. Deposit\n2. Withdraw\n3. Transfer\n4. View Action History\n5. Back");
            Console.Write("\nSelect an operation : ");
            string userInput = Console.ReadLine();
            try
            {
                operation = (Option5)(Convert.ToInt32(userInput));
            }
            catch
            {
                operation = (Option5)0;
            }
            return operation;
        }
    }
}
