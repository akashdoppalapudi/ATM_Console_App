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

        public string CreateNewBank(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new BankCreationFailedException();
            }
            if (this.banks.FindAll(b => b.IsActive).Exists(b => b.Name == name))
            {
                throw new BankNameAlreadyExistsException();
            }
            Bank newBank = new Bank
            {
                Name = name,
                Id = idGenService.GenBankId(name),
                Accounts = new List<Account>(),
                Employees = new List<Employee>(),
                IsActive = true,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                DeletedOn = null
            };
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
            if (bank.Employees.FindAll(e => e.IsActive).Exists(e => e.Username == employeeDetails.Item3))
            {
                throw new UsernameAlreadyExistsException();
            }
            if (employeeDetails == null)
            {
                throw new AccountCreationFailedException();
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

        public string UpdateBank(string bankId, string employeeId, string name)
        {
            Bank bank = this.banks.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            Employee employee = bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
            if (employee == null || employee.EmployeeType != EmployeeType.Admin)
            {
                throw new AccessDeniedException();
            }
            if (this.banks.FindAll(b => b.IsActive && b.Id != bankId).Exists(b => b.Name == name))
            {
                throw new BankNameAlreadyExistsException();
            }
            bank.Name = name;
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
            updateEmployee.Password = encryptionService.ComputeSha256Hash(employeeDetails.Item4);
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
            account.Password = accountDetails.Item4;
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
            Employee deleteEmployee = bank.Employees.FindAll(e => e.IsActive).Find(e => e.Id == deleteEmployeeId);
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
            Employee employee = bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive); ;
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
            foreach (Bank bank in this.banks)
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

        public string CheckEmployeeExistance(string bankId, string username)
        {
            Employee employee = this.banks.Find(b => b.Id == bankId && b.IsActive).Employees.FirstOrDefault(e => e.Username == username && e.IsActive);
            if (employee == null)
            {
                throw new EmployeeDoesNotExistException();
            }
            return employee.Id;
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
    }
}
