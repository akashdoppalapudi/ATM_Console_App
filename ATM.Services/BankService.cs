using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.Exceptions;
using System.Collections.Generic;

namespace ATM.Services
{
    public class BankService
    {
        private readonly IDGenService idGenService;
        private readonly TransactionService transactionService;
        private readonly EmployeeActionService employeeActionService;
        private readonly EmployeeService employeeService;
        private readonly AccountService accountService;
        private readonly CurrencyService currencyService;
        private readonly DBService dbService;

        public BankService()
        {
            transactionService = new TransactionService();
            employeeActionService = new EmployeeActionService();
            idGenService = new IDGenService();
            employeeService = new EmployeeService();
            accountService = new AccountService();
            currencyService = new CurrencyService();
            dbService = new DBService();
        }

        public void CheckBankExistance(string bankId)
        {
            dbService.CheckBankExistance(bankId);
        }

        public Bank CreateBank(string name)
        {
            return new Bank
            {
                Name = name,
                Id = idGenService.GenId(name)
            };
        }

        public void AddBank(Bank bank, Employee adminEmployee)
        {
            dbService.AddBank(bank);
            employeeService.AddEmployee(bank.Id, adminEmployee);
            Currency defaultCurrency = currencyService.CreateCurrency("INR", 1);
            currencyService.AddCurrency(bank.Id, defaultCurrency);
        }

        public void AddEmployee(string bankId, string employeeId, Employee newEmployee)
        {
            if (!employeeService.IsEmployeeAdmin(bankId, employeeId))
            {
                throw new AccessDeniedException();
            }
            employeeService.AddEmployee(bankId, newEmployee);
            EmployeeAction action = employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.NewAccount, newEmployee.Id);
            employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void AddAccount(string bankId, string employeeId, Account newAccount)
        {
            Transaction transaction = transactionService.CreateTransaction(bankId, newAccount.Id, 1500, TransactionType.Credit, TransactionNarrative.AccountCreation, newAccount.Id);
            accountService.AddAccount(bankId, newAccount);
            transactionService.AddTransaction(bankId, newAccount.Id, transaction);
            EmployeeAction action = employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.NewAccount, newAccount.Id, transaction.Id);
            employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void UpdateBank(string bankId, string employeeId, Bank updateBank)
        {
            Bank bank = dbService.GetBankById(bankId);
            if (!employeeService.IsEmployeeAdmin(bankId, employeeId))
            {
                throw new AccessDeniedException();
            }
            bank.Name = updateBank.Name;
            bank.IMPS = updateBank.IMPS;
            bank.RTGS = updateBank.RTGS;
            bank.OIMPS = updateBank.OIMPS;
            bank.ORTGS = updateBank.ORTGS;
            dbService.UpdateBank(bank);
            EmployeeAction action = employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.UpdateBank);
            employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void UpdateEmployee(string bankId, string employeeId, string currentEmployeeId, Employee updateEmployee)
        {
            if (!employeeService.IsEmployeeAdmin(bankId, employeeId))
            {
                throw new AccessDeniedException();
            }
            employeeService.UpdateEmployee(bankId, currentEmployeeId, updateEmployee);
            EmployeeAction action = employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.UpdateAccount, currentEmployeeId);
            employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void UpdateAccount(string bankId, string employeeId, string currentAccountId, Account updateAccount)
        {
            accountService.UpdateAccount(bankId, currentAccountId, updateAccount);
            EmployeeAction action = employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.UpdateAccount, currentAccountId);
            employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void DeleteBank(string bankId, string employeeId)
        {
            if (!employeeService.IsEmployeeAdmin(bankId, employeeId))
            {
                throw new AccessDeniedException();
            }
            dbService.DeleteBank(bankId);
            EmployeeAction action = employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.DeleteBank);
            employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void DeleteEmployee(string bankId, string employeeId, string deleteEmployeeId)
        {
            if (!employeeService.IsEmployeeAdmin(bankId, employeeId))
            {
                throw new AccessDeniedException();
            }
            employeeService.DeleteEmployee(bankId, deleteEmployeeId);
            EmployeeAction action = employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.DeleteAccount, deleteEmployeeId);
            employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void DeleteAccount(string bankId, string employeeId, string accountId)
        {
            accountService.DeleteAccount(bankId, accountId);
            EmployeeAction action = employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.DeleteAccount, accountId);
            employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public Dictionary<string, string> GetAllBankNames()
        {
            return dbService.GetAllBankNames();
        }

        public void Deposit(string bankId, string accountId, Currency currency, decimal amount)
        {
            accountService.Deposit(bankId, accountId, currency, amount);
            Transaction transaction = transactionService.CreateTransaction(bankId, accountId, amount, TransactionType.Credit, TransactionNarrative.Deposit, accountId);
            transactionService.AddTransaction(bankId, accountId, transaction);
        }

        public void Withdraw(string bankId, string accountId, decimal amount)
        {
            accountService.Withdraw(bankId, accountId, amount);
            Transaction transaction = transactionService.CreateTransaction(bankId, accountId, amount, TransactionType.Debit, TransactionNarrative.Withdraw, accountId);
            transactionService.AddTransaction(bankId, accountId, transaction);
        }

        public void Transfer(string selectedBankId, string selectedAccountId, string transferToBankId, string transferToAccountId, decimal amount)
        {
            accountService.Transfer(selectedBankId, selectedAccountId, transferToBankId, transferToAccountId, amount);
            Transaction fromTransaction = transactionService.CreateTransaction(selectedBankId, selectedAccountId, amount, TransactionType.Debit, TransactionNarrative.Transfer, selectedAccountId, transferToBankId, transferToAccountId);
            transactionService.AddTransaction(selectedBankId, selectedAccountId, fromTransaction);
            Transaction toTransaction = transactionService.CreateTransaction(transferToBankId, transferToAccountId, amount, TransactionType.Credit, TransactionNarrative.Transfer, selectedAccountId, transferToBankId, transferToAccountId);
            transactionService.AddTransaction(transferToBankId, transferToAccountId, toTransaction);
        }

        public void RevertTransaction(string bankId, string employeeId, string txnId)
        {
            Transaction transaction = transactionService.GetTransactionById(bankId, txnId);
            decimal amount = transaction.TransactionAmount;
            string fromAccId = transaction.AccountId;
            string toAccId = transaction.ToAccountId;
            string toBankId = transaction.ToBankId;
            Transfer(toBankId, toAccId, bankId, fromAccId, amount);
            EmployeeAction action = employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.RevertTransaction, fromAccId, txnId);
            employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void AddCurrency(string bankId, string employeeId, Currency currency)
        {
            currencyService.AddCurrency(bankId, currency);
            EmployeeAction action = employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.UpdateBank);
            employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void UpdateCurrency(string bankId, string employeeId, string currencyName, Currency updateCurrency)
        {
            currencyService.UpdateCurrency(bankId, currencyName, updateCurrency);
            EmployeeAction action = employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.UpdateBank);
            employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void DeleteCurrency(string bankId, string employeeId, string currencyName)
        {
            currencyService.DeleteCurrency(bankId, currencyName);
            EmployeeAction action = employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.UpdateBank);
            employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void ValidateBankName(string bankName)
        {
            dbService.ValidateBankName(bankName);
        }

        public Bank GetBankDetails(string bankId)
        {
            Bank bank = dbService.GetBankById(bankId);
            return bank;
        }
    }
}
