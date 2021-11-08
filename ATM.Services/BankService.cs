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
        private EmployeeService employeeService;
        private AccountService accountService;

        public BankService()
        {
            transactionHandler = new TransactionHandler();
            encryptionService = new EncryptionService();
            dataHandler = new DataHandler();
            idGenService = new IDGenService();
            employeeService = new EmployeeService();
            accountService = new AccountService();
            this.banks = dataHandler.ReadBankData();
            if (this.banks == null)
            {
                this.banks = new List<Bank>();
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
            bank.Employees.Add(adminEmployee);
            bank.Currencies.Add(new Currency { Name = "INR", ExchangeRate = 1 });
            bank.UpdatedOn = DateTime.Now;
            this.banks.Add(bank);
            dataHandler.WriteBankData(this.banks);
        }

        private Bank GetBankById(string bankId)
        {
            return this.banks.FirstOrDefault(b => b.Id == bankId && b.IsActive);
        }

        private Employee GetEmployeeById(Bank bank, string employeeId)
        {
            return bank.Employees.FirstOrDefault(e => e.Id == employeeId && e.IsActive);
        }

        private Account GetAccountById(Bank bank, string accountId)
        {
            return bank.Accounts.FirstOrDefault(a => a.Id == accountId && a.IsActive);
        }

        public void AddEmployee(string bankId, string employeeId, Employee newEmployee)
        {
            Bank bank = GetBankById(bankId);
            Employee employee = GetEmployeeById(bank, employeeId);
            if (!employeeService.IsEmployeeAdmin(employee))
            {
                throw new AccessDeniedException();
            }
            if (bank.Employees.FindAll(e => e.IsActive).Exists(e => e.Username == newEmployee.Username))
            {
                throw new UsernameAlreadyExistsException();
            }
            EmployeeAction action = transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employee.Id), EmployeeActionType.NewAccount, newEmployee.Id);
            employeeService.AddAction(employee, action);
            bank.Employees.Add(newEmployee);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void AddAccount(string bankId, string employeeId, Account newAccount)
        {
            Bank bank = GetBankById(bankId);
            Employee employee = GetEmployeeById(bank, employeeId);
            if (bank.Accounts.FindAll(a => a.IsActive).Exists(a => a.Username == newAccount.Username))
            {
                throw new UsernameAlreadyExistsException();
            }
            Transaction transaction = transactionHandler.NewTransaction(idGenService.GenTransactionId(bankId, newAccount.Id), 1500, TransactionType.Credit, TransactionNarrative.AccountCreation, newAccount.Id);
            accountService.AddTransaction(newAccount, transaction);
            bank.Accounts.Add(newAccount);
            EmployeeAction action = transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), EmployeeActionType.NewAccount, newAccount.Id, transaction.Id);
            employeeService.AddAction(employee, action);
            employee.UpdatedOn = DateTime.Now;
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void UpdateBank(string bankId, string employeeId, Bank updateBank)
        {
            Bank bank = GetBankById(bankId);
            Employee employee = GetEmployeeById(bank, employeeId);
            if (employeeService.IsEmployeeAdmin(employee))
            {
                throw new AccessDeniedException();
            }
            if (this.banks.FindAll(b => b.IsActive && b.Id != bankId).Exists(b => b.Name == updateBank.Name))
            {
                throw new BankNameAlreadyExistsException();
            }
            bank.Name = updateBank.Name;
            bank.IMPS = updateBank.IMPS;
            bank.RTGS = updateBank.RTGS;
            bank.OIMPS = updateBank.OIMPS;
            bank.ORTGS = updateBank.ORTGS;
            EmployeeAction action = transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), EmployeeActionType.UpdateBank);
            employeeService.AddAction(employee, action);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void UpdateEmployee(string bankId, string employeeId, string currentEmployeeId, Employee updateEmployee)
        {
            Bank bank = GetBankById(bankId);
            Employee employee = GetEmployeeById(bank, employeeId);
            Employee currentEmployee = GetEmployeeById(bank, currentEmployeeId);
            if (employeeService.IsEmployeeAdmin(employee))
            {
                throw new AccessDeniedException();
            }
            if (bank.Employees.FindAll(e => e.IsActive && e.Id != currentEmployeeId).Exists(e => e.Username == updateEmployee.Username))
            {
                throw new UsernameAlreadyExistsException();
            }
            employeeService.UpdateEmployee(currentEmployee, updateEmployee);
            EmployeeAction action = transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), EmployeeActionType.UpdateAccount, currentEmployee.Id);
            employeeService.AddAction(employee, action);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void UpdateAccount(string bankId, string employeeId, string currentAccountId, Account updateAccount)
        {
            Bank bank = GetBankById(bankId);
            if (bank.Accounts.FindAll(a => a.IsActive && a.Id != currentAccountId).Exists(a => a.Username == updateAccount.Username))
            {
                throw new UsernameAlreadyExistsException();
            }
            Employee employee = GetEmployeeById(bank, employeeId);
            Account currentAccount = GetAccountById(bank, currentAccountId);
            accountService.UpdateAccount(currentAccount, updateAccount);
            EmployeeAction action = transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), EmployeeActionType.UpdateAccount, currentAccount.Id);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void DeleteBank(string bankId, string employeeId)
        {
            Bank bank = GetBankById(bankId);
            Employee employee = GetEmployeeById(bank, employeeId);
            if (employeeService.IsEmployeeAdmin(employee))
            {
                throw new AccessDeniedException();
            }
            EmployeeAction action = transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), EmployeeActionType.DeleteBank);
            employeeService.AddAction(employee, action);
            bank.IsActive = false;
            bank.UpdatedOn = DateTime.Now;
            bank.DeletedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void DeleteEmployee(string bankId, string employeeId, string deleteEmployeeId)
        {
            Bank bank = GetBankById(bankId);
            Employee employee = GetEmployeeById(bank, employeeId);
            if (employeeService.IsEmployeeAdmin(employee))
            {
                throw new AccessDeniedException();
            }
            Employee deleteEmployee = GetEmployeeById(bank, deleteEmployeeId);
            employeeService.DeleteEmployee(deleteEmployee);
            EmployeeAction action = transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), EmployeeActionType.DeleteAccount, deleteEmployeeId);
            employeeService.AddAction(employee, action);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void DeleteAccount(string bankId, string employeeId, string accountId)
        {
            Bank bank = GetBankById(bankId);
            Employee employee = GetEmployeeById(bank, employeeId);
            Account account = GetAccountById(bank, accountId);
            accountService.DeleteAccount(account);
            EmployeeAction action = transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), (EmployeeActionType)3, accountId);
            employeeService.AddAction(employee, action);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
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

        public void CheckBankExistance(string bankId)
        {
            if (this.banks.Exists(b => b.Id == bankId && b.IsActive))
            {
                return;
            }
            throw new BankDoesnotExistException();
        }

        public string GetEmployeeIdByUsername(string bankId, string username)
        {
            Employee employee = GetBankById(bankId).Employees.FirstOrDefault(e => e.Username == username && e.IsActive);
            if (employee == null)
            {
                throw new EmployeeDoesNotExistException();
            }
            return employee.Id;
        }

        public string GetAccountIdByUsername(string bankId, string username)
        {
            Account account = GetBankById(bankId).Accounts.FirstOrDefault(a => a.Username == username && a.IsActive);
            if (account == null)
            {
                throw new AccountDoesNotExistException();
            }
            return account.Id;
        }

        public decimal GetBalance(string bankId, string accountId)
        {
            Bank bank = GetBankById(bankId);
            Account account = GetAccountById(bank, accountId);
            return account.Balance;
        }

        public List<Transaction> GetTransactions(string bankId, string accountId)
        {
            Bank bank = GetBankById(bankId);
            Account account = GetAccountById(bank, accountId);
            return account.Transactions;
        }

        public List<EmployeeAction> GetEmployeeActions(string bankId, string employeeId)
        {
            Bank bank = GetBankById(bankId);
            Employee employee = GetEmployeeById(bank, employeeId);
            return employee.EmployeeActions;
        }

        public void Deposit(string bankId, string accountId, Currency currency, decimal amount)
        {
            Bank bank = GetBankById(bankId);
            Account account = GetAccountById(bank, accountId);
            if (amount <= 0)
            {
                throw new InvalidAmountException();
            }
            amount *= (decimal)currency.ExchangeRate;
            accountService.Deposit(account, amount);
            Transaction transaction = transactionHandler.NewTransaction(idGenService.GenTransactionId(bankId, accountId), amount, TransactionType.Credit, TransactionNarrative.Deposit, accountId);
            accountService.AddTransaction(account, transaction);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void Withdraw(string bankId, string accountId, decimal amount)
        {
            Bank bank = GetBankById(bankId);
            Account account = GetAccountById(bank, accountId);
            if (amount <= 0 || amount > account.Balance)
            {
                throw new InvalidAmountException();
            }
            accountService.Withdraw(account, amount);
            Transaction transaction = transactionHandler.NewTransaction(idGenService.GenTransactionId(bankId, accountId), amount, TransactionType.Debit, TransactionNarrative.Withdraw, accountId);
            accountService.AddTransaction(account, transaction);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void Transfer(string selectedBankId, string selectedAccountId, string transferToBankId, string transferToAccountId, decimal amount)
        {
            if (selectedAccountId == transferToAccountId && selectedBankId == transferToBankId)
            {
                throw new AccessDeniedException();
            }
            Bank bank = GetBankById(selectedBankId);
            Bank toBank = GetBankById(transferToBankId);
            Account account = GetAccountById(bank, selectedAccountId);
            Account transferToAccount = GetAccountById(toBank, transferToAccountId);
            if (amount <= 0 || amount > account.Balance)
            {
                throw new InvalidAmountException();
            }
            accountService.Transfer(account, transferToAccount, amount);
            string TXNId = idGenService.GenTransactionId(selectedBankId, selectedAccountId);
            Transaction fromTransaction = transactionHandler.NewTransaction(idGenService.GenTransactionId(selectedBankId, selectedAccountId), amount, TransactionType.Debit, TransactionNarrative.Transfer, selectedAccountId, transferToBankId, transferToAccountId);
            accountService.AddTransaction(account, fromTransaction);
            bank.UpdatedOn = DateTime.Now;
            Transaction toTransaction = transactionHandler.NewTransaction(idGenService.GenTransactionId(transferToBankId, transferToAccountId), amount, TransactionType.Credit, TransactionNarrative.Transfer, selectedAccountId, transferToBankId, transferToAccountId);
            accountService.AddTransaction(transferToAccount, toTransaction);
            toBank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void RevertTransaction(string bankId, string employeeId, string txnId)
        {
            Bank bank = GetBankById(bankId);
            Employee employee = GetEmployeeById(bank, employeeId);
            List<Transaction> transactions = new List<Transaction>();
            foreach (Account account in bank.Accounts)
            {
                List<Transaction> possibleTransactions = account.Transactions.FindAll(t => t.Id == txnId && t.TransactionType == TransactionType.Debit && t.TransactionNarrative == TransactionNarrative.Transfer);
                transactions.AddRange(possibleTransactions);
            }
            if (transactions.Count <= 0)
            {
                throw new TransactionNotFoundException();
            }
            transactions = transactions.OrderBy(t => t.TransactionDate).ToList();
            Transaction transaction = transactions.Last();
            decimal amount = transaction.TransactionAmount;
            string fromAccId = transaction.FromAccountId;
            string toAccId = transaction.ToAccountId;
            string toBankId = transaction.ToBankId;
            Transfer(toBankId, toAccId, bankId, fromAccId, amount);
            EmployeeAction action = transactionHandler.NewEmployeeAction(idGenService.GenEmployeeActionId(bankId, employeeId), EmployeeActionType.RevertTransaction, fromAccId, txnId);
            employeeService.AddAction(employee, action);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public void AddCurrency(string bankId, string currencyName, double exchangeRate)
        {
            if (String.IsNullOrEmpty(currencyName) || exchangeRate <= 0)
            {
                throw new CurrencyDataInvalidException();
            }
            Bank bank = GetBankById(bankId);
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
            Bank bank = GetBankById(bankId);
            Currency currency = GetCurrencyByName(bankId, currencyName);
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
            Bank bank = GetBankById(bankId);
            Currency currency = GetCurrencyByName(bankId, currencyName);
            if (currency == null)
            {
                throw new CurrencyDoesNotExistException();
            }
            bank.Currencies.Remove(currency);
            bank.UpdatedOn = DateTime.Now;
            dataHandler.WriteBankData(this.banks);
        }

        public Currency GetCurrencyByName(string bankId, string currencyName)
        {
            Bank bank = banks.Find(b => b.Id == bankId && b.IsActive);
            Currency currency = bank.Currencies.FirstOrDefault(c => c.Name == currencyName);
            if (currency == null)
            {
                throw new CurrencyDoesNotExistException();
            }
            return currency;
        }

        public void AuthenticateUser(string bankId, string accountId, string userInput)
        {
            Bank bank = GetBankById(bankId);
            Account account = GetAccountById(bank, accountId);
            if (accountService.Authenticate(account, userInput))
            {
                throw new AuthenticationFailedException();
            }
        }

        public void AuthenticateEmployee(string bankId, string employeeId, string userInput)
        {
            Bank bank = GetBankById(bankId);
            Employee employee = GetEmployeeById(bank, employeeId);
            if (employeeService.Authenticate(employee, userInput))
            {
                throw new AuthenticationFailedException();
            }
        }

        public Bank GetBankDetails(string bankId)
        {
            Bank bank = GetBankById(bankId);
            return new Bank
            {
                Name = bank.Name,
                IMPS = bank.IMPS,
                RTGS = bank.RTGS,
                OIMPS = bank.OIMPS,
                ORTGS = bank.ORTGS
            };
        }

        public Employee GetEmployeeDetails(string bankId, string employeeId)
        {
            Bank bank = GetBankById(bankId);
            Employee employee = GetEmployeeById(bank, employeeId);
            return new Employee
            {
                Name = employee.Name,
                Gender = employee.Gender,
                Username = employee.Username,
                EmployeeType = employee.EmployeeType
            };
        }

        public Account GetAccountDetails(string bankId, string accountId)
        {
            Bank bank = GetBankById(bankId);
            Account account = GetAccountById(bank, accountId);
            return new Account
            {
                Name = account.Name,
                Gender = account.Gender,
                Username = account.Username,
                AccountType = account.AccountType
            };
        }
    }
}
