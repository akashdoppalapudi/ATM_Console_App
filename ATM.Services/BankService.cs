using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using ATM.Services.IServices;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class BankService : IBankService
    {
        private readonly ITransactionService _transactionService;
        private readonly IEmployeeActionService _employeeActionService;
        private readonly IEmployeeService _employeeService;
        private readonly IAccountService _accountService;
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;
        private readonly BankContext _bankContext;

        public BankService(ITransactionService transactionService, IEmployeeActionService employeeActionService, IEmployeeService employeeService, IAccountService accountService, ICurrencyService currencyService, BankContext bankContext, IMapper mapper)
        {
            _transactionService = transactionService;
            _employeeActionService = employeeActionService;
            _employeeService = employeeService;
            _accountService = accountService;
            _currencyService = currencyService;
            _mapper = mapper;
            _bankContext = bankContext;
            _bankContext.Database.EnsureCreated();
        }

        public void CheckBankExistance(string bankId)
        {
            if (!_bankContext.Bank.Any(b => b.Id == bankId && b.IsActive))
            {
                throw new BankDoesnotExistException();
            }
        }

        public Bank GetBankById(string bankId)
        {
            CheckBankExistance(bankId);
            BankDBModel bankRecord = _bankContext.Bank.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            return _mapper.Map<Bank>(bankRecord);
        }

        public Bank CreateBank(string name)
        {
            return new Bank
            {
                Name = name,
                Id = name.GenId()
            };
        }

        public void AddBank(Bank bank, Employee adminEmployee)
        {
            BankDBModel bankRecord = _mapper.Map<BankDBModel>(bank);
            _bankContext.Bank.Add(bankRecord);
            _bankContext.SaveChanges();
            _employeeService.AddEmployee(bank.Id, adminEmployee);
            Currency defaultCurrency = _currencyService.CreateCurrency("INR", 1);
            _currencyService.AddCurrency(bank.Id, defaultCurrency);
        }

        public void AddEmployee(string bankId, string employeeId, Employee newEmployee)
        {
            if (!_employeeService.IsEmployeeAdmin(bankId, employeeId))
            {
                throw new AccessDeniedException();
            }
            _employeeService.AddEmployee(bankId, newEmployee);
            EmployeeAction action = _employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.NewAccount, newEmployee.Id);
            _employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void AddAccount(string bankId, string employeeId, Account newAccount)
        {
            // what happens if the adding of account fails at backend so better add transaction after the operation
            Transaction transaction = _transactionService.CreateTransaction(bankId, newAccount.Id, 1500, TransactionType.Credit, TransactionNarrative.AccountCreation, newAccount.Id);
            _accountService.AddAccount(bankId, newAccount);
            _transactionService.AddTransaction(bankId, newAccount.Id, transaction);
            EmployeeAction action = _employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.NewAccount, newAccount.Id, transaction.Id);
            _employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void UpdateBank(string bankId, string employeeId, Bank updateBank)
        {
            if (!_employeeService.IsEmployeeAdmin(bankId, employeeId))
            {
                throw new AccessDeniedException();
            }
            BankDBModel currentBankRecord = _bankContext.Bank.First(b => b.Id == bankId && b.IsActive);
            currentBankRecord.Name = updateBank.Name;
            currentBankRecord.IMPS = updateBank.IMPS;
            currentBankRecord.RTGS = updateBank.RTGS;
            currentBankRecord.OIMPS = updateBank.OIMPS;
            currentBankRecord.ORTGS = updateBank.ORTGS;
            _bankContext.SaveChanges();
            EmployeeAction action = _employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.UpdateBank);
            _employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void UpdateEmployee(string bankId, string employeeId, string currentEmployeeId, Employee updateEmployee)
        {
            if (!_employeeService.IsEmployeeAdmin(bankId, employeeId))
            {
                throw new AccessDeniedException();
            }
            _employeeService.UpdateEmployee(bankId, currentEmployeeId, updateEmployee);
            EmployeeAction action = _employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.UpdateAccount, currentEmployeeId);
            _employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void UpdateAccount(string bankId, string employeeId, string currentAccountId, Account updateAccount)
        {
            _accountService.UpdateAccount(bankId, currentAccountId, updateAccount);
            EmployeeAction action = _employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.UpdateAccount, currentAccountId);
            _employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void DeleteBank(string bankId, string employeeId)
        {
            if (!_employeeService.IsEmployeeAdmin(bankId, employeeId))
            {
                throw new AccessDeniedException();
            }
            BankDBModel bankRecord = _bankContext.Bank.First(b => b.Id == bankId && b.IsActive);
            bankRecord.IsActive = false;
            bankRecord.DeletedOn = DateTime.Now;
            var employeeRecords = _bankContext.Employee.Where(e => e.BankId == bankId && e.IsActive).ToList();
            employeeRecords.ForEach(e => e.IsActive = false);
            var accountRecords = _bankContext.Account.Where(a => a.BankId == bankId && a.IsActive).ToList();
            accountRecords.ForEach(a => a.IsActive = false);
            _bankContext.Currency.RemoveRange(_bankContext.Currency.Where(c => c.BankId == bankId));
            _bankContext.SaveChanges();
            EmployeeAction action = _employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.DeleteBank);
            _employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void DeleteEmployee(string bankId, string employeeId, string deleteEmployeeId)
        {
            if (!_employeeService.IsEmployeeAdmin(bankId, employeeId))
            {
                throw new AccessDeniedException();
            }
            _employeeService.DeleteEmployee(bankId, deleteEmployeeId);
            EmployeeAction action = _employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.DeleteAccount, deleteEmployeeId);
            _employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void DeleteAccount(string bankId, string employeeId, string accountId)
        {
            _accountService.DeleteAccount(bankId, accountId);
            EmployeeAction action = _employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.DeleteAccount, accountId);
            _employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public Dictionary<string, string> GetAllBankNames()
        {
            Dictionary<string, string> bankNames = new Dictionary<string, string>();
            var bankRecords = _bankContext.Bank.Where(b => b.IsActive).Select(b => new { b.Id, b.Name });
            foreach (var bankRecord in bankRecords)
            {
                bankNames.Add(bankRecord.Id, bankRecord.Name);
            }
            return bankNames;
        }

        public void Deposit(string bankId, string accountId, Currency currency, decimal amount)
        {
            _accountService.Deposit(bankId, accountId, currency, amount);
            Transaction transaction = _transactionService.CreateTransaction(bankId, accountId, amount * ((decimal)currency.ExchangeRate), TransactionType.Credit, TransactionNarrative.Deposit, accountId);
            _transactionService.AddTransaction(bankId, accountId, transaction);
        }

        public void Withdraw(string bankId, string accountId, decimal amount)
        {
            _accountService.Withdraw(bankId, accountId, amount);
            Transaction transaction = _transactionService.CreateTransaction(bankId, accountId, amount, TransactionType.Debit, TransactionNarrative.Withdraw, accountId);
            _transactionService.AddTransaction(bankId, accountId, transaction);
        }

        public void Transfer(string selectedBankId, string selectedAccountId, string transferToBankId, string transferToAccountId, decimal amount)
        {
            _accountService.Transfer(selectedBankId, selectedAccountId, transferToBankId, transferToAccountId, amount);
            Transaction fromTransaction = _transactionService.CreateTransaction(selectedBankId, selectedAccountId, amount, TransactionType.Debit, TransactionNarrative.Transfer, selectedAccountId, transferToBankId, transferToAccountId);
            _transactionService.AddTransaction(selectedBankId, selectedAccountId, fromTransaction);
            Transaction toTransaction = _transactionService.CreateTransaction(transferToBankId, transferToAccountId, amount, TransactionType.Credit, TransactionNarrative.Transfer, selectedAccountId, transferToBankId, transferToAccountId);
            _transactionService.AddTransaction(transferToBankId, transferToAccountId, toTransaction);
        }

        public void RevertTransaction(string bankId, string employeeId, string txnId)
        {
            Transaction transaction = _transactionService.GetTransactionById(bankId, txnId);
            decimal amount = transaction.TransactionAmount;
            string fromAccId = transaction.AccountId;
            string toAccId = transaction.ToAccountId;
            string toBankId = transaction.ToBankId;
            Transfer(toBankId, toAccId, bankId, fromAccId, amount);
            EmployeeAction action = _employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.RevertTransaction, fromAccId, txnId);
            _employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void AddCurrency(string bankId, string employeeId, Currency currency)
        {
            _currencyService.AddCurrency(bankId, currency);
            EmployeeAction action = _employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.UpdateBank);
            _employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void UpdateCurrency(string bankId, string employeeId, string currencyName, Currency updateCurrency)
        {
            _currencyService.UpdateCurrency(bankId, currencyName, updateCurrency);
            EmployeeAction action = _employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.UpdateBank);
            _employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void DeleteCurrency(string bankId, string employeeId, string currencyName)
        {
            _currencyService.DeleteCurrency(bankId, currencyName);
            EmployeeAction action = _employeeActionService.CreateEmployeeAction(bankId, employeeId, EmployeeActionType.UpdateBank);
            _employeeActionService.AddEmployeeAction(bankId, employeeId, action);
        }

        public void ValidateBankName(string bankName)
        {
            if (_bankContext.Bank.Any(b => b.Name == bankName && b.IsActive))
            {
                throw new BankNameAlreadyExistsException();
            }
        }

        public Bank GetBankDetails(string bankId)
        {
            CheckBankExistance(bankId);
            BankDBModel bankRecord = _bankContext.Bank.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            return _mapper.Map<Bank>(bankRecord);
        }
    }
}
