using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System;
using AutoMapper;

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
        private readonly MapperConfiguration bankDBConfig;
        private readonly Mapper bankDBMapper;
        private readonly MapperConfiguration dbBankConfig;
        private readonly Mapper dbBankMapper;

        public BankService()
        {
            transactionService = new TransactionService();
            employeeActionService = new EmployeeActionService();
            idGenService = new IDGenService();
            employeeService = new EmployeeService();
            accountService = new AccountService();
            currencyService = new CurrencyService();
            bankDBConfig = new MapperConfiguration(cfg => cfg.CreateMap<Bank, BankDBModel>());
            bankDBMapper = new Mapper(bankDBConfig);
            dbBankConfig = new MapperConfiguration(cfg => cfg.CreateMap<BankDBModel, Bank>());
            dbBankMapper = new Mapper(dbBankConfig);
        }

        public void CheckBankExistance(string bankId)
        {
            using (BankContext bankContext = new BankContext())
            {
                if (!bankContext.Bank.Any(b => b.Id==bankId && b.IsActive))
                {
                    throw new BankDoesnotExistException();
                }
            }
        }

        public Bank GetBankById(string bankId)
        {
            CheckBankExistance(bankId);
            using (BankContext bankContext = new BankContext())
            {
                BankDBModel bankRecord = bankContext.Bank.FirstOrDefault(b => b.Id == bankId && b.IsActive);
                return dbBankMapper.Map<Bank>(bankRecord);
            }
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
            BankDBModel bankRecord = bankDBMapper.Map<BankDBModel>(bank);
            using (BankContext bankContext = new BankContext())
            {
                bankContext.Bank.Add(bankRecord);
                bankContext.SaveChanges();
            }
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
            if (!employeeService.IsEmployeeAdmin(bankId, employeeId))
            {
                throw new AccessDeniedException();
            }
            using (BankContext bankContext = new BankContext())
            {
                BankDBModel currentBankRecord = bankContext.Bank.First(b => b.Id == bankId && b.IsActive);
                currentBankRecord.Name = updateBank.Name;
                currentBankRecord.IMPS = updateBank.IMPS;
                currentBankRecord.RTGS = updateBank.RTGS;
                currentBankRecord.OIMPS = updateBank.OIMPS;
                currentBankRecord.ORTGS = updateBank.ORTGS;
                bankContext.SaveChanges();
            }
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
            using (BankContext bankContext = new BankContext())
            {
                BankDBModel bankRecord = bankContext.Bank.First(b => b.Id == bankId && b.IsActive);
                bankRecord.IsActive = false;
                bankRecord.DeletedOn = DateTime.Now;
                var employeeRecords = bankContext.Employee.Where(e => e.BankId == bankId && e.IsActive).ToList();
                employeeRecords.ForEach(e => e.IsActive = false);
                var accountRecords = bankContext.Account.Where(a => a.BankId == bankId && a.IsActive).ToList();
                accountRecords.ForEach(a => a.IsActive = false);
                bankContext.Currency.RemoveRange(bankContext.Currency.Where(c => c.BankId == bankId));
                bankContext.SaveChanges();
            }
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
            Dictionary<string, string> bankNames = new Dictionary<string, string>();
            using (BankContext bankContext = new BankContext())
            {
                var bankRecords = bankContext.Bank.Where(b => b.IsActive).Select(b => new { b.Id, b.Name });
                foreach (var bankRecord in bankRecords)
                {
                    bankNames.Add(bankRecord.Id, bankRecord.Name);
                }
            }
            return bankNames;
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
            using (BankContext bankContext = new BankContext())
            {
                if (bankContext.Bank.Any(b => b.Name == bankName && b.IsActive))
                {
                    throw new BankNameAlreadyExistsException();
                }
            }
        }

        public Bank GetBankDetails(string bankId)
        {
            CheckBankExistance(bankId);
            using (BankContext bankContext = new BankContext())
            {
                BankDBModel bankRecord = bankContext.Bank.FirstOrDefault(b => b.Id == bankId && b.IsActive);
                return dbBankMapper.Map<Bank>(bankRecord);
            }
        }
    }
}
