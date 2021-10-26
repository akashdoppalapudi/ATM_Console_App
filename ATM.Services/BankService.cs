using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class BankService
    {
        private List<Bank> banks;
        private DataHandler dataHandler;
        private EncryptionService encryptionService;
        private IDGenService idGenService;
        private TransactionHandler transactionHandler;

        public BankService()
        {
            transactionHandler = new TransactionHandler();
            encryptionService = new EncryptionService();
            dataHandler = new DataHandler();
            idGenService = new IDGenService();
            this.banks = dataHandler.ReadBankData();
            if (this.banks == null)
            {
                this.banks = new List<Bank>();
            }
        }

        public string CreateNewBank(Tuple<string> bankDetails, Tuple<string, Gender, string, string> employeeDetails)
        {
            if (bankDetails == null)
            {
                throw new BankCreationFailedException();
            }
            if (employeeDetails == null)
            {
                throw new AccountCreationFailedException();
            }
            if (this.banks.FindAll(b => b.IsActive).Exists(b => b.Name == bankDetails.Item1))
            {
                throw new BankNameAlreadyExistsException();
            }
            Employee newEmployee = new Employee
            {
                Id = idGenService.GenEmployeeId(employeeDetails.Item1),
                Name = employeeDetails.Item1,
                Gender = employeeDetails.Item2,
                Username = employeeDetails.Item3,
                Password = encryptionService.ComputeSha256Hash(employeeDetails.Item4),
                EmployeeType = EmployeeType.Admin,
                IsActive = true,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                DeletedOn = null,
                EmployeeActions = new List<EmployeeAction>()
            };
            Bank newBank = new Bank
            {
                Name = bankDetails.Item1,
                Id = idGenService.GenBankId(bankDetails.Item1),
                Accounts = new List<Account>(),
                Employees = new List<Employee>(),
                Currencies = new List<Currency>(),
                IsActive = true,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                DeletedOn = null
            };
            newBank.Employees.Add(newEmployee);
            newBank.Currencies.Add(new Currency
            {
                Name = "INR",
                ExchangeRate = 1
            });
            this.banks.Add(newBank);
            dataHandler.WriteBankData(this.banks);
            return newBank.Id;
        }

        public Tuple<string, EmployeeType> CreateNewEmployee(string bankId, string employeeId, Tuple<string, Gender, string, string, EmployeeType> employeeDetails)
        {
            Bank bank = this.banks.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            Employee employee = bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employee == null || employee.EmployeeType != EmployeeType.Admin)
            {
                throw new AccessDeniedException();
            }
            if (employeeDetails == null)
            {
                throw new AccountCreationFailedException();
            }
            if (bank.Employees.FindAll(e => e.IsActive).Exists(e => e.Username == employeeDetails.Item3))
            {
                throw new UsernameAlreadyExistsException();
            }
            Employee newEmployee = new Employee
            {
                Id = idGenService.GenEmployeeId(employeeDetails.Item1),
                Name = employeeDetails.Item1,
                Gender = employeeDetails.Item2,
                Username = employeeDetails.Item3,
                Password = encryptionService.ComputeSha256Hash(employeeDetails.Item4),
                EmployeeType = employeeDetails.Item5,
                IsActive = true,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                DeletedOn = null,
                EmployeeActions = new List<EmployeeAction>()
            };
            newEmployee.EmployeeActions.Add(transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, newEmployee.Id), (EmployeeActionType)1, newEmployee.Id));
            bank.Employees.Add(newEmployee);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
            return Tuple.Create(newEmployee.Id, newEmployee.EmployeeType);
        }

        public string CreateNewAccount(string bankId, string employeeId, Tuple<string, Gender, string, string, AccountType> accountDetails)
        {
            Bank bank = this.banks.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            Employee employee = bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employee == null)
            {
                throw new AccessDeniedException();
            }
            if (bank.Accounts.FindAll(a => a.IsActive).Exists(a => a.Username == accountDetails.Item3))
            {
                throw new UsernameAlreadyExistsException();
            }
            if (accountDetails == null)
            {
                throw new AccountCreationFailedException();
            }
            Account newAccount = new Account
            {
                Id = idGenService.GenAccountId(accountDetails.Item1),
                Name = accountDetails.Item1,
                Gender = accountDetails.Item2,
                Username = accountDetails.Item3,
                Password = encryptionService.ComputeSha256Hash(accountDetails.Item4),
                AccountType = accountDetails.Item5,
                IsActive = true,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                DeletedOn = null,
                Balance = 1500,
                Transactions = new List<Transaction>()
            };
            Transaction newTransaction = transactionHandler.NewTransaction(idGenService.GenTransactionId(bankId, newAccount.Id), 1500, (TransactionType)2, (TransactionNarrative)1, newAccount.Id);
            newAccount.Transactions.Add(newTransaction);
            bank.Accounts.Add(newAccount);
            employee.EmployeeActions.Add(transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), (EmployeeActionType)1, newAccount.Id, newTransaction.Id));
            employee.UpdatedOn = DateTime.Now;
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
            return newAccount.Id;
        }

        public string UpdateBank(string bankId, string employeeId, Tuple<string> bankDetails)
        {
            Bank bank = this.banks.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            Employee employee = bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employee == null || employee.EmployeeType != EmployeeType.Admin)
            {
                throw new AccessDeniedException();
            }
            if (this.banks.FindAll(b => b.IsActive && b.Id != bankId).Exists(b => b.Name == bankDetails.Item1))
            {
                throw new BankNameAlreadyExistsException();
            }
            bank.Name = bankDetails.Item1;
            employee.EmployeeActions.Add(transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), (EmployeeActionType)4));
            employee.UpdatedOn = DateTime.Now;
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
            return bankId;
        }

        public string UpdateEmployee(string bankId, string employeeId, string updateEmployeeId, Tuple<string, Gender, string, string, EmployeeType> employeeDetails)
        {
            Bank bank = this.banks.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            Employee employee = bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employee == null || employee.EmployeeType != EmployeeType.Admin)
            {
                throw new AccessDeniedException();
            }
            if (bank.Employees.FindAll(e => e.IsActive && e.Id != updateEmployeeId).Exists(e => e.Username == employeeDetails.Item3))
            {
                throw new UsernameAlreadyExistsException();
            }
            Employee updateEmployee = bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            updateEmployee.Name = employeeDetails.Item1;
            updateEmployee.Gender = employeeDetails.Item2;
            updateEmployee.Username = employeeDetails.Item3;
            if (!String.IsNullOrEmpty(employeeDetails.Item4))
            {
                updateEmployee.Password = encryptionService.ComputeSha256Hash(employeeDetails.Item4);
            }
            updateEmployee.EmployeeType = employeeDetails.Item5;
            updateEmployee.UpdatedOn = DateTime.Now;
            employee.EmployeeActions.Add(transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), (EmployeeActionType)2, updateEmployee.Id));
            employee.UpdatedOn = DateTime.Now;
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
            return updateEmployeeId;
        }

        public string UpdateAccount(string bankId, string employeeId, string accountId, Tuple<string, Gender, string, string, AccountType> accountDetails)
        {
            Bank bank = this.banks.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            if (bank.Accounts.FindAll(a => a.IsActive && a.Id != accountId).Exists(a => a.Username == accountDetails.Item3))
            {
                throw new UsernameAlreadyExistsException();
            }
            Employee employee = bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employee == null)
            {
                throw new AccessDeniedException();
            }
            Account account = bank.Accounts.FirstOrDefault(a => a.Id == accountId && a.IsActive);
            account.Name = accountDetails.Item1;
            account.Gender = accountDetails.Item2;
            account.Username = accountDetails.Item3;
            if (!String.IsNullOrEmpty(accountDetails.Item4))
            {
                account.Password = accountDetails.Item4;
            }
            account.AccountType = accountDetails.Item5;
            account.UpdatedOn = DateTime.Now;
            employee.EmployeeActions.Add(transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), (EmployeeActionType)2, account.Id));
            employee.UpdatedOn = DateTime.Now;
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
            return accountId;
        }

        public string DeleteBank(string bankId, string employeeId)
        {
            Bank bank = this.banks.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            Employee employee = bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employee == null || employee.EmployeeType != EmployeeType.Admin)
            {
                throw new AccessDeniedException();
            }
            employee.EmployeeActions.Add(transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), (EmployeeActionType)5));
            employee.UpdatedOn = DateTime.Now;
            bank.IsActive = false;
            bank.UpdatedOn = DateTime.Now;
            bank.DeletedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
            return bankId;
        }

        public string DeleteEmployee(string bankId, string employeeId, string deleteEmployeeId)
        {
            Bank bank = this.banks.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            Employee employee = bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employee == null || employee.EmployeeType != EmployeeType.Admin)
            {
                throw new AccessDeniedException();
            }
            Employee deleteEmployee = bank.Employees.FirstOrDefault(e => e.Id == deleteEmployeeId && e.IsActive);
            deleteEmployee.IsActive = false;
            deleteEmployee.UpdatedOn = DateTime.Now;
            deleteEmployee.DeletedOn = DateTime.Now;
            employee.EmployeeActions.Add(transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), (EmployeeActionType)3, deleteEmployeeId));
            employee.UpdatedOn = DateTime.Now;
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
            return deleteEmployeeId;
        }

        public string DeleteAccount(string bankId, string employeeId, string accountId)
        {
            Bank bank = this.banks.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            Employee employee = bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employee == null)
            {
                throw new AccessDeniedException();
            }
            Account account = bank.Accounts.FirstOrDefault(a => a.Id == accountId && a.IsActive);
            account.IsActive = false;
            account.UpdatedOn = DateTime.Now;
            account.DeletedOn = DateTime.Now;
            employee.EmployeeActions.Add(transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), (EmployeeActionType)3, accountId));
            employee.UpdatedOn = DateTime.Now;
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
            return accountId;
        }

        public Dictionary<string, string> GetAllBankNames()
        {
            Dictionary<string, string> bankNames = new Dictionary<string, string>();
            foreach (Bank bank in this.banks.FindAll(b => b.IsActive))
            {
                bankNames.Add(bank.Id, bank.Name);
            }
            return bankNames;
        }

        public string CheckBankExistance(string bankId)
        {
            if (this.banks.Exists(b => b.Id == bankId && b.IsActive))
            {
                return bankId;
            }
            throw new BankDoesnotExistException();
        }

        public Tuple<string, EmployeeType> CheckEmployeeExistance(string bankId, string username)
        {
            Employee employee = this.banks.Find(b => b.Id == bankId && b.IsActive).Employees.FirstOrDefault(e => e.Username == username && e.IsActive);
            if (employee == null)
            {
                throw new EmployeeDoesNotExistException();
            }
            return Tuple.Create(employee.Id, employee.EmployeeType);
        }

        public string CheckAccountExistance(string bankId, string username)
        {
            Account account = this.banks.Find(b => b.Id == bankId && b.IsActive).Accounts.FirstOrDefault(a => a.Username == username && a.IsActive);
            if (account == null)
            {
                throw new AccountDoesNotExistException();
            }
            return account.Id;
        }

        public decimal GetBalance(string bankId, string accountId)
        {
            Bank bank = this.banks.Find(b => b.Id == bankId && b.IsActive);
            Account account = bank.Accounts.Find(a => a.Id == accountId && a.IsActive);
            return account.Balance;
        }

        public List<Transaction> GetTransactions(string bankId, string accountId)
        {
            Bank bank = this.banks.Find(b => b.Id == bankId && b.IsActive);
            Account account = bank.Accounts.Find(a => a.Id == accountId && a.IsActive);
            return account.Transactions;
        }

        public List<EmployeeAction> GetEmployeeActions(string bankId, string employeeId)
        {
            Bank bank = this.banks.Find(b => b.Id == bankId && b.IsActive);
            Employee employee = bank.Employees.Find(e => e.Id == employeeId && e.IsActive);
            return employee.EmployeeActions;
        }

        public string Deposit(string bankId, string accountId, Currency currency, decimal amount)
        {
            Bank bank = this.banks.Find(b => b.Id == bankId && b.IsActive);
            Account account = bank.Accounts.Find(a => a.Id == accountId && a.IsActive);
            if (amount <= 0)
            {
                throw new InvalidAmountException();
            }
            amount = amount * (decimal)currency.ExchangeRate;
            account.Balance += amount;
            string TXNId = idGenService.GenTransactionId(bankId, accountId);
            account.Transactions.Add(transactionHandler.NewTransaction(TXNId, amount, (TransactionType)2, (TransactionNarrative)2, accountId));
            account.UpdatedOn = DateTime.Now;
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
            return TXNId;
        }

        public string Withdraw(string bankId, string accountId, Currency currency, decimal amount)
        {
            Bank bank = this.banks.Find(b => b.Id == bankId && b.IsActive);
            Account account = bank.Accounts.Find(a => a.Id == accountId && a.IsActive);
            if (amount <= 0 || amount > account.Balance)
            {
                throw new InvalidAmountException();
            }
            amount = amount * (decimal)currency.ExchangeRate;
            account.Balance -= amount;
            string TXNId = idGenService.GenTransactionId(bankId, accountId);
            account.Transactions.Add(transactionHandler.NewTransaction(TXNId, amount, (TransactionType)1, (TransactionNarrative)3, accountId));
            account.UpdatedOn = DateTime.Now;
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
            return TXNId;
        }

        public string Transfer(string selectedBankId, string selectedAccountId, string transferToBankId, string transferToAccountId, Currency currency, decimal amount)
        {
            if (selectedAccountId == transferToAccountId && selectedBankId == transferToBankId)
            {
                throw new AccessDeniedException();
            }
            Bank bank = this.banks.Find(b => b.Id == selectedBankId && b.IsActive);
            Bank toBank = this.banks.Find(b => b.Id == transferToBankId && b.IsActive);
            Account account = bank.Accounts.Find(a => a.Id == selectedAccountId && a.IsActive);
            Account transferToAccount = toBank.Accounts.Find(a => a.Id == transferToAccountId && a.IsActive);
            if (amount <= 0 || amount > account.Balance)
            {
                throw new InvalidAmountException();
            }
            amount = amount * (decimal)currency.ExchangeRate;
            account.Balance -= amount;
            string TXNId = idGenService.GenTransactionId(selectedBankId, selectedAccountId);
            account.Transactions.Add(transactionHandler.NewTransaction(TXNId, amount, (TransactionType)1, (TransactionNarrative)3, selectedAccountId, transferToBankId, transferToAccountId));
            account.UpdatedOn = DateTime.Now;
            bank.UpdatedOn = DateTime.Now;
            transferToAccount.Balance += amount;
            transferToAccount.Transactions.Add(transactionHandler.NewTransaction(idGenService.GenTransactionId(transferToBankId, transferToAccountId), amount, (TransactionType)2, (TransactionNarrative)3, selectedAccountId, transferToBankId, transferToAccountId));
            transferToAccount.UpdatedOn = DateTime.Now;
            toBank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
            return TXNId;
        }

        public void AddCurrency(string bankId, string currencyName, double exchangeRate)
        {
            if (String.IsNullOrEmpty(currencyName) || exchangeRate <= 0)
            {
                throw new CurrencyDataInvalidException();
            }
            Bank bank = this.banks.Find(b => b.Id == bankId && b.IsActive);
            if (bank.Currencies.Exists(c => c.Name == currencyName))
            {
                throw new CurrencyAlreadyExistsException();
            }
            Currency newCurrency = new Currency
            {
                Name = currencyName,
                ExchangeRate = exchangeRate
            };
            bank.Currencies.Add(newCurrency);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void UpdateCurrency(string bankId, string currencyName, double exchangeRate)
        {
            if (String.IsNullOrEmpty(currencyName) || exchangeRate <= 0)
            {
                throw new CurrencyDataInvalidException();
            }
            Bank bank = this.banks.Find(b => b.Id == bankId && b.IsActive);
            Currency currency = bank.Currencies.FirstOrDefault(c => c.Name == currencyName);
            if (currency == null)
            {
                throw new CurrencyDoesNotExistException();
            }
            currency.ExchangeRate = exchangeRate;
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void DeleteCurrency(string bankId, string currencyName)
        {
            if (String.IsNullOrEmpty(currencyName))
            {
                throw new CurrencyDataInvalidException();
            }
            Bank bank = this.banks.Find(b => b.Id == bankId && b.IsActive);
            Currency currency = bank.Currencies.FirstOrDefault(c => c.Name == currencyName);
            if (currency == null)
            {
                throw new CurrencyDoesNotExistException();
            }
            bank.Currencies.Remove(currency);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public Currency CheckCurrencyExistance(string bankId, string currencyName)
        {
            Bank bank = this.banks.Find(b => b.Id == bankId && b.IsActive);
            Currency currency = bank.Currencies.FirstOrDefault(c => c.Name == currencyName);
            if (currency == null)
            {
                throw new CurrencyDoesNotExistException();
            }
            return currency;
        }

        public void AuthenticateUser(string bankId, string accountId, string userInput)
        {
            Bank bank = this.banks.Find(b => b.Id == bankId && b.IsActive);
            Account account = bank.Accounts.Find(a => a.Id == accountId && a.IsActive);
            string hashedUserInput = encryptionService.ComputeSha256Hash(userInput);
            if (hashedUserInput != account.Password)
            {
                throw new AuthenticationFailedException();
            }
        }

        public void AuthenticateEmployee(string bankId, string employeeId, string userInput)
        {
            Bank bank = this.banks.Find(b => b.Id == bankId && b.IsActive);
            Employee employee = bank.Employees.Find(e => e.Id == employeeId && e.IsActive);
            string hashedUserInput = encryptionService.ComputeSha256Hash(userInput);
            if (hashedUserInput != employee.Password)
            {
                throw new AuthenticationFailedException();
            }
        }

        public Tuple<string> GetBankDetails(string bankId)
        {
            Bank bank = this.banks.Find(b => b.Id == bankId && b.IsActive);
            return Tuple.Create(bank.Name);
        }

        public Tuple<string, Gender, string, EmployeeType> GetEmployeeDetails(string bankId, string employeeId)
        {
            Bank bank = this.banks.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            Employee employee = bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            return Tuple.Create(employee.Name, employee.Gender, employee.Username, employee.EmployeeType);
        }

        public Tuple<string, Gender, string, AccountType> GetAccountDetails(string bankId, string accountId)
        {
            Bank bank = this.banks.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            Account account = bank.Accounts.FirstOrDefault(a => a.Id == accountId && a.IsActive);
            return Tuple.Create(account.Name, account.Gender, account.Username, account.AccountType);
        }
    }
}
