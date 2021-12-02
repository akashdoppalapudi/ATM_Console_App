using ATM.Models;
using ATM.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    internal class DBService
    {
        public DBService()
        {
            using (BankContext bankContext = new BankContext())
            {
                bankContext.Database.EnsureCreated();
            }
        }


        public void AddBank(Bank bank)
        {
            using (BankContext bankContext = new BankContext())
            {
                bankContext.Bank.Add(bank);
                bankContext.SaveChanges();
            }
        }

        public void AddEmployee(Employee employee)
        {
            using (BankContext bankContext = new BankContext())
            {
                bankContext.Employee.Add(employee);
                bankContext.SaveChanges();
            }
        }

        public void AddCurrency(Currency currency)
        {
            using (BankContext bankContext = new BankContext())
            {
                bankContext.Currency.Add(currency);
                bankContext.SaveChanges();
            }
        }

        public void AddAccount(Account account)
        {
            using (BankContext bankContext = new BankContext())
            {
                bankContext.Account.Add(account);
                bankContext.SaveChanges();
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            using (BankContext bankContext = new BankContext())
            {
                bankContext.Transaction.Add(transaction);
                bankContext.SaveChanges();
            }
        }

        public void AddEmployeeAction(EmployeeAction employeeAction)
        {
            using (BankContext bankContext = new BankContext())
            {
                bankContext.EmployeeAction.Add(employeeAction);
                bankContext.SaveChanges();
            }
        }

        public Dictionary<string, string> GetAllBankNames()
        {
            Dictionary<string, string> bankNames = new Dictionary<string, string>();
            using (BankContext bankContext = new BankContext())
            {
                var banks = bankContext.Bank.Where(b => b.IsActive).Select(b => new { b.Id, b.Name });
                foreach (var bank in banks)
                {
                    bankNames.Add(bank.Id, bank.Name);
                }
            }
            return bankNames;
        }

        public string GetAccountIdByUsername(string bankId, string username)
        {
            string id;
            using (BankContext bankContext = new BankContext())
            {
                Account account = bankContext.Account.FirstOrDefault(a => a.BankId == bankId && a.IsActive && a.Username == username);
                if (account == null)
                {
                    throw new AccountDoesNotExistException();
                }
                id = account.Id;
            }
            return id;
        }

        public string GetEmployeeIdByUsername(string bankId, string username)
        {
            string id;
            using (BankContext bankContext = new BankContext())
            {
                Employee employee = bankContext.Employee.FirstOrDefault(e => e.BankId == bankId && e.IsActive && e.Username == username);
                if (employee == null)
                {
                    throw new EmployeeDoesNotExistException();
                }
                id = employee.Id;
            }
            return id;
        }

        public void CheckBankExistance(string bankId)
        {
            using (BankContext bankContext = new BankContext())
            {
                if (!bankContext.Bank.Any(b => b.Id == bankId && b.IsActive))
                {
                    throw new BankDoesnotExistException();
                }
            }
        }

        public void CheckCurrencyExistance(string bankId, string currencyName)
        {
            using (BankContext bankContext = new BankContext())
            {
                if (!bankContext.Currency.Any(c => c.BankId == bankId && c.Name == currencyName))
                {
                    throw new CurrencyDoesNotExistException();
                }
            }
        }

        public void CheckEmployeeExistance(string bankId, string employeeId)
        {
            using (BankContext bankContext = new BankContext())
            {
                if (!bankContext.Employee.Any(e => e.BankId == bankId && e.Id == employeeId && e.IsActive))
                {
                    throw new EmployeeDoesNotExistException();
                }
            }
        }

        public void CheckAccountExistance(string bankId, string accountId)
        {
            using (BankContext bankContext = new BankContext())
            {
                if (!bankContext.Account.Any(a => a.BankId == bankId && a.Id == accountId && a.IsActive))
                {
                    throw new AccountDoesNotExistException();
                }
            }
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

        public void ValidateAccountUsername(string bankId, string username)
        {
            using (BankContext bankContext = new BankContext())
            {
                if (bankContext.Account.Any(a => a.BankId == bankId && a.Username == username && a.IsActive))
                {
                    throw new UsernameAlreadyExistsException();
                }
            }
        }

        public void ValidateEmployeeUsername(string bankId, string username)
        {
            using (BankContext bankContext = new BankContext())
            {
                if (bankContext.Employee.Any(e => e.BankId == bankId && e.Username == username && e.IsActive))
                {
                    throw new UsernameAlreadyExistsException();
                }
            }
        }

        public void ValidateCurrencyName(string bankId, string currencyName)
        {
            using (BankContext bankContext = new BankContext())
            {
                if (bankContext.Currency.Any(c => c.BankId == bankId && c.Name == currencyName))
                {
                    throw new CurrencyAlreadyExistsException();
                }
            }
        }

        public Bank GetBankById(string bankId)
        {
            CheckBankExistance(bankId);
            using (BankContext bankContext = new BankContext())
            {
                return bankContext.Bank.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            }
        }

        public Employee GetEmployeeById(string bankId, string employeeId)
        {
            CheckEmployeeExistance(bankId, employeeId);
            using (BankContext bankContext = new BankContext())
            {
                return bankContext.Employee.FirstOrDefault(e => e.BankId == bankId && e.Id == employeeId && e.IsActive);
            }
        }

        public Account GetAccountById(string bankId, string accountId)
        {
            CheckAccountExistance(bankId, accountId);
            using (BankContext bankContext = new BankContext())
            {
                return bankContext.Account.FirstOrDefault(a => a.BankId == bankId && a.Id == accountId && a.IsActive);
            }
        }

        public Currency GetCurrencyByName(string bankId, string currencyName)
        {
            CheckCurrencyExistance(bankId, currencyName);
            using (BankContext bankContext = new BankContext())
            {
                return bankContext.Currency.FirstOrDefault(c => c.BankId == bankId && c.Name == currencyName);
            }
        }

        public Transaction GetTransactionById(string bankId, string txnId)
        {
            using (BankContext bankContext = new BankContext())
            {
                Transaction transaction = bankContext.Transaction.FirstOrDefault(t => t.BankId == bankId && t.Id == txnId);
                if (transaction == null)
                {
                    throw new TransactionNotFoundException();
                }
                return transaction;
            }
        }

        public IList<Transaction> GetTransactions(string bankId, string accountId)
        {
            IList<Transaction> transactions;
            using (BankContext bankContext = new BankContext())
            {
                transactions = bankContext.Transaction.Where(t => t.BankId == bankId && t.AccountId == accountId).ToList();
            }
            if (transactions.Count == 0 || transactions == null)
            {
                throw new NoTransactionsException();
            }
            return transactions;
        }

        public IList<EmployeeAction> GetEmployeeActions(string bankId, string employeeId)
        {
            IList<EmployeeAction> actions;
            using (BankContext bankContext = new BankContext())
            {
                actions = bankContext.EmployeeAction.Where(a => a.BankId == bankId && a.EmployeeId == employeeId).ToList();
            }
            if (actions.Count == 0 || actions == null)
            {
                throw new NoEmployeeActionsException();
            }
            return actions;
        }

        public void UpdateBank(Bank bank)
        {
            using (BankContext bankContext = new BankContext())
            {
                Bank currentBank = bankContext.Bank.First(b => b.Id == bank.Id && bank.IsActive);
                currentBank.Name = bank.Name;
                currentBank.IMPS = bank.IMPS;
                currentBank.RTGS = bank.RTGS;
                currentBank.OIMPS = bank.OIMPS;
                currentBank.ORTGS = bank.ORTGS;
                bankContext.SaveChanges();
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            using (BankContext bankContext = new BankContext())
            {
                Employee currentEmployee = bankContext.Employee.First(e => e.BankId == employee.BankId && e.Id == employee.Id && e.IsActive);
                currentEmployee.Name = employee.Name;
                currentEmployee.Gender = employee.Gender;
                currentEmployee.Username = employee.Username;
                currentEmployee.Password = employee.Password;
                currentEmployee.Salt = employee.Salt;
                currentEmployee.EmployeeType = employee.EmployeeType;
                bankContext.SaveChanges();
            }
        }

        public void UpdateAccount(Account account)
        {
            using (BankContext bankContext = new BankContext())
            {
                Account currentAccount = bankContext.Account.First(a => a.BankId == account.BankId && a.Id == account.Id && a.IsActive);
                currentAccount.Name = account.Name;
                currentAccount.Gender = account.Gender;
                currentAccount.Username = account.Username;
                currentAccount.Password = account.Password;
                currentAccount.Salt = account.Salt;
                currentAccount.AccountType = account.AccountType;
                currentAccount.Balance = account.Balance;
                bankContext.SaveChanges();
            }
        }

        public void UpdateCurrency(Currency currency)
        {
            using (BankContext bankContext = new BankContext())
            {
                Currency currentCurrency = bankContext.Currency.First(c => c.BankId==currency.BankId && c.Name==currency.Name);
                currentCurrency.ExchangeRate = currency.ExchangeRate;
                bankContext.SaveChanges();
            }
        }

        public void DeleteBank(string bankId)
        {
            using (BankContext bankContext = new BankContext())
            {
                Bank bank = bankContext.Bank.First(b => b.Id==bankId && b.IsActive);
                bank.IsActive = false;
                var employees = bankContext.Employee.Where(e => e.BankId == bankId && e.IsActive).ToList();
                employees.ForEach(e => e.IsActive = false);
                var accounts = bankContext.Account.Where(a => a.BankId == bankId && a.IsActive).ToList();
                accounts.ForEach(a => a.IsActive = false);
                bankContext.Currency.RemoveRange(bankContext.Currency.Where(c => c.BankId == bankId));
                bankContext.SaveChanges();
            }
        }

        public void DeleteAccount(string bankId, string accountId)
        {
            using (BankContext bankContext = new BankContext())
            {
                Account account = bankContext.Account.First(a => a.Id == accountId && a.BankId == bankId && a.IsActive);
                account.IsActive = false;
                bankContext.SaveChanges();
            }
        }

        public void DeleteEmployee(string bankId, string employeeId)
        {
            using (BankContext bankContext = new BankContext())
            {
                Employee employee = bankContext.Employee.First(e => e.Id == employeeId && e.BankId==bankId && e.IsActive);
                employee.IsActive = false;
                bankContext.SaveChanges();
            }
        }

        public void DeleteCurrency(string bankId, string currencyName)
        {
            using (BankContext bankContext = new BankContext())
            {
                bankContext.Remove(bankContext.Currency.First(c => c.BankId == bankId && c.Name == currencyName));
                bankContext.SaveChanges();
            }
        }
    }
}
