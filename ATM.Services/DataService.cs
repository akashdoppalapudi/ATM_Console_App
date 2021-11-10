using ATM.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ATM.Services
{
    public class DataService
    {
        private readonly string bankDataFile = "bankdata.json";
        private readonly string accountDataFile = "accountdata.json";
        private readonly string employeeDataFile = "employeedata.json";
        private readonly string currencyDataFile = "currencydata.json";
        private readonly string transactionDataFile = "transactiondata.json";
        private readonly string employeeActionDataFile = "employeeactiondata.json";
        private JsonSerializerOptions indentOption = new JsonSerializerOptions() { WriteIndented = true };

        public IList<Bank> ReadBankData()
        {
            if (File.Exists(bankDataFile))
            {
                IList<Bank> retrievedBanks;
                try
                {
                    string jsonString = File.ReadAllText(bankDataFile);
                    retrievedBanks = JsonSerializer.Deserialize<IList<Bank>>(jsonString);
                }
                catch
                {
                    retrievedBanks = null;
                }
                return retrievedBanks;
            }
            else
            {
                return null;
            }
        }

        public IList<Account> ReadAccountData()
        {
            if (File.Exists(accountDataFile))
            {
                IList<Account> retrievedAccounts;
                try
                {
                    string jsonString = File.ReadAllText(accountDataFile);
                    retrievedAccounts = JsonSerializer.Deserialize<IList<Account>>(jsonString);
                }
                catch
                {
                    retrievedAccounts = null;
                }
                return retrievedAccounts;
            }
            else
            {
                return null;
            }
        }

        public IList<Employee> ReadEmployeeData()
        {
            if (File.Exists(employeeDataFile))
            {
                IList<Employee> retrievedEmployees;
                try
                {
                    string jsonString = File.ReadAllText(employeeDataFile);
                    retrievedEmployees = JsonSerializer.Deserialize<IList<Employee>>(jsonString);
                }
                catch
                {
                    retrievedEmployees = null;
                }
                return retrievedEmployees;
            }
            else
            {
                return null;
            }
        }

        public IList<Currency> ReadCurrencyData()
        {
            if (File.Exists(currencyDataFile))
            {
                IList<Currency> retrievedCurrencies;
                try
                {
                    string jsonString = File.ReadAllText(currencyDataFile);
                    retrievedCurrencies = JsonSerializer.Deserialize<IList<Currency>>(jsonString);
                }
                catch
                {
                    retrievedCurrencies = null;
                }
                return retrievedCurrencies;
            }
            else
            {
                return null;
            }
        }

        public IList<Transaction> ReadTransactionData()
        {
            if (File.Exists(transactionDataFile))
            {
                IList<Transaction> retrievedTransactions;
                try
                {
                    string jsonString = File.ReadAllText(transactionDataFile);
                    retrievedTransactions = JsonSerializer.Deserialize<IList<Transaction>>(jsonString);
                }
                catch
                {
                    retrievedTransactions = null;
                }
                return retrievedTransactions;
            }
            else
            {
                return null;
            }
        }

        public IList<EmployeeAction> ReadEmployeeActions()
        {
            if (File.Exists(employeeActionDataFile))
            {
                IList<EmployeeAction> retrievedEmployeeActions;
                try
                {
                    string jsonString = File.ReadAllText(employeeActionDataFile);
                    retrievedEmployeeActions = JsonSerializer.Deserialize<IList<EmployeeAction>>(jsonString);
                }
                catch
                {
                    retrievedEmployeeActions = null;
                }
                return retrievedEmployeeActions;
            }
            else
            {
                return null;
            }
        }

        public void WriteBankData(IList<Bank> updatedBanks)
        {
            string jsonString = JsonSerializer.Serialize<IList<Bank>>(updatedBanks, indentOption);
            File.WriteAllText(bankDataFile, jsonString);
        }
    }
}
