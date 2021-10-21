using System;
using System.Collections.Generic;
using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.Exceptions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (this.banks.Exists(b => b.Name == name))
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

        public Tuple<string, EmployeeType> CreateNewEmployee(string bankId, Tuple<string, Gender, string, string, EmployeeType> employeeDetails)
        {
            Bank bank = this.banks.FindAll(b => b.IsActive).Find(b => b.Id == bankId);
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
                Password = employeeDetails.Item4,
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
            Bank bank = this.banks.FindAll(b => b.IsActive).Find(b => b.Id == bankId);
            Employee employee = bank.Employees.FindAll(e => e.IsActive).Find(e => e.Id == employeeId);
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
                Password = accountDetails.Item4,
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

        public string UpdateEmployee(string bankId, string employeeId, string updateEmployeeId, Tuple<string, Gender, string, string, EmployeeType> employeeDetails)
        {
            Bank bank = this.banks.FindAll(b => b.IsActive).Find(b => b.Id == bankId);
            if (bank.Employees.FindAll(e => e.IsActive&&e.Id!=updateEmployeeId).Exists(e => e.Username == employeeDetails.Item3))
            {
                throw new UsernameAlreadyExistsException();
            }
            Employee employee = bank.Employees.FindAll(e => e.IsActive).Find(e => e.Id == employeeId);
            Employee updateEmployee = bank.Employees.FindAll(e => e.IsActive).Find(e => e.Id == updateEmployeeId);
            updateEmployee.Name = employeeDetails.Item1;
            updateEmployee.Gender = employeeDetails.Item2;
            updateEmployee.Username = employeeDetails.Item3;
            updateEmployee.Password = employeeDetails.Item4;
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
            Bank bank = this.banks.FindAll(b => b.IsActive).Find(b => b.Id == bankId);
            if (bank.Accounts.FindAll(a => a.IsActive&&a.Id!=accountId).Exists(a => a.Username == accountDetails.Item3))
            {
                throw new UsernameAlreadyExistsException();
            }
            Employee employee = bank.Employees.FindAll(e => e.IsActive).Find(e => e.Id == employeeId);
            Account account = bank.Accounts.FindAll(a => a.IsActive).Find(e => e.Id == employeeId);
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
    }
}
