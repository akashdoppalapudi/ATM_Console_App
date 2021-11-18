using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.Exceptions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ATM.Services
{
    internal class DBService
    {
        private readonly string connectionString = @"server=localhost;port=3306;username=root;password=Akash@1729;database=banking_application_data;";
        private readonly string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        private readonly MySqlConnection conn;
        private readonly MySqlCommand cmd;

        public DBService()
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            conn = connection;
            using MySqlCommand command = new MySqlCommand();
            command.Connection = conn;
            cmd = command;
        }

        public void AddBank(Bank bank)
        {
            conn.Open();
            cmd.CommandText = $"INSERT INTO banks(id,name,imps,rtgs,oimps,ortgs,created_on,is_active) VALUES ('{bank.Id}','{bank.Name}',{bank.IMPS},{bank.RTGS},{bank.OIMPS},{bank.ORTGS},'{bank.CreatedOn.ToString(dateTimeFormat)}',{bank.IsActive});";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void AddEmployee(Employee employee)
        {
            conn.Open();
            cmd.CommandText = $"INSERT INTO persons(id,name,gender,username,password,is_active,created_on) VALUES ('{employee.Id}','{employee.Name}',{(int)employee.Gender},'{employee.Username}','{employee.Password}',{employee.IsActive},'{employee.CreatedOn.ToString(dateTimeFormat)}');";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"INSERT INTO employees(id,bank_id,type) VALUES ('{employee.Id}','{employee.BankId}',{(int)employee.EmployeeType});";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void AddCurrency(Currency currency)
        {
            conn.Open();
            cmd.CommandText = $"INSERT INTO currencies(bank_id,name,exchange_rate) VALUES ('{currency.BankId}','{currency.Name}',{currency.ExchangeRate});";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void AddAccount(Account account)
        {
            conn.Open();
            cmd.CommandText = $"INSERT INTO persons(id,name,gender,username,password,is_active,created_on) VALUES ('{account.Id}','{account.Name}',{(int)account.Gender},'{account.Username}','{account.Password}',{account.IsActive},'{account.CreatedOn.ToString(dateTimeFormat)}');";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"INSERT INTO accounts(id,bank_id,type,balance) VALUES ('{account.Id}','{account.BankId}',{(int)account.AccountType},{account.Balance});";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void AddTransaction(Transaction transaction)
        {
            conn.Open();
            if (String.IsNullOrEmpty(transaction.ToAccountId) || String.IsNullOrEmpty(transaction.ToBankId))
            {
                cmd.CommandText = $"INSERT INTO transactions(id,account_id,bank_id,date,type,narrative,amount) VALUES ('{transaction.Id}','{transaction.AccountId}','{transaction.BankId}','{transaction.TransactionDate.ToString(dateTimeFormat)}',{(int)transaction.TransactionType},{(int)transaction.TransactionNarrative},{transaction.TransactionAmount});";
            }
            else
            {
                cmd.CommandText = $"INSERT INTO transactions(id,account_id,bank_id,date,type,to_bank_id,to_account_id,narrative,amount) VALUES ('{transaction.Id}','{transaction.AccountId}','{transaction.BankId}','{transaction.TransactionDate.ToString(dateTimeFormat)}',{(int)transaction.TransactionType},'{transaction.ToBankId}','{transaction.ToAccountId}',{(int)transaction.TransactionNarrative},{transaction.TransactionAmount});";
            }
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void AddEmployeeAction(EmployeeAction employeeAction)
        {
            conn.Open();
            if (String.IsNullOrEmpty(employeeAction.AccountId))
            {
                cmd.CommandText = $"INSERT INTO actions(id,bank_id,employee_id,date,type) VALUES ('{employeeAction.Id}','{employeeAction.BankId}','{employeeAction.EmployeeId}','{employeeAction.ActionDate.ToString(dateTimeFormat)}',{(int)employeeAction.ActionType});";
            }
            else
            {
                if (String.IsNullOrEmpty(employeeAction.TXNId))
                {
                    cmd.CommandText = $"INSERT INTO actions(id,bank_id,employee_id,account_id,date,type) VALUES ('{employeeAction.Id}','{employeeAction.BankId}','{employeeAction.EmployeeId}','{employeeAction.AccountId}','{employeeAction.ActionDate.ToString(dateTimeFormat)}',{(int)employeeAction.ActionType});";
                }
                else
                {
                    cmd.CommandText = $"INSERT INTO actions(id,bank_id,employee_id,txn_id,account_id,date,type) VALUES ('{employeeAction.Id}','{employeeAction.BankId}','{employeeAction.EmployeeId}','{employeeAction.TXNId}','{employeeAction.AccountId}','{employeeAction.ActionDate.ToString(dateTimeFormat)}',{(int)employeeAction.ActionType});";
                }
            }
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public Dictionary<string, string> GetAllBankNames()
        {
            Dictionary<string, string> bankNames = new Dictionary<string, string>();
            conn.Open();
            cmd.CommandText = @"SELECT id,name FROM banks;";
            using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                bankNames.Add(rdr.GetString(0), rdr.GetString(1));
            }
            conn.Close();
            return bankNames;
        }

        public string GetAccountIdByUsername(string bankId, string username)
        {
            string id;
            conn.Open();
            cmd.CommandText = $"SELECT COUNT(*) FROM accounts INNER JOIN persons ON accounts.id=persons.id WHERE accounts.bank_id='{bankId}' AND persons.username='{username}' AND persons.is_active=TRUE;";
            if (Convert.ToInt32(cmd.ExecuteScalar()) <= 0)
            {
                conn.Close();
                throw new AccountDoesNotExistException();
            }
            cmd.CommandText = $"SELECT accounts.id FROM accounts INNER JOIN persons ON accounts.id=persons.id WHERE accounts.bank_id='{bankId}' AND persons.username='{username}' AND persons.is_active=TRUE;";
            try
            {
                id = cmd.ExecuteScalar().ToString();
            }
            catch (NullReferenceException)
            {
                conn.Close();
                throw new EmployeeDoesNotExistException();
            }
            conn.Close();
            return id;
        }

        public string GetEmployeeIdByUsername(string bankId, string username)
        {
            string id;
            conn.Open();
            cmd.CommandText = $"SELECT COUNT(*) FROM employees INNER JOIN persons ON employees.id=persons.id WHERE employees.bank_id='{bankId}' AND persons.username='{username}' AND persons.is_active=TRUE;";
            if (Convert.ToInt32(cmd.ExecuteScalar()) <= 0)
            {
                conn.Close();
                throw new EmployeeDoesNotExistException();
            }
            cmd.CommandText = $"SELECT employees.id FROM employees INNER JOIN persons ON employees.id=persons.id WHERE employees.bank_id='{bankId}' AND persons.username='{username}' AND persons.is_active=TRUE;";
            try
            {
                id = cmd.ExecuteScalar().ToString();
            }
            catch (NullReferenceException)
            {
                conn.Close();
                throw new AccountDoesNotExistException();
            }
            conn.Close();
            return id;
        }

        public void CheckBankExistance(string bankId)
        {
            conn.Open();
            cmd.CommandText = $"SELECT COUNT(*) FROM banks WHERE id='{bankId}' AND is_active=TRUE;";
            if (Convert.ToInt32(cmd.ExecuteScalar()) <= 0)
            {
                conn.Close();
                throw new BankDoesnotExistException();
            }
            conn.Close();
        }

        public void CheckCurrencyExistance(string bankId, string currencyName)
        {
            conn.Open();
            cmd.CommandText = $"SELECT COUNT(*) FROM currencies WHERE bank_id='{bankId}' AND name='{currencyName}';";
            if (Convert.ToInt32(cmd.ExecuteScalar()) <= 0)
            {
                conn.Close();
                throw new CurrencyDoesNotExistException();
            }
            conn.Close();
        }

        public void CheckEmployeeExistance(string bankId, string employeeId)
        {
            conn.Open();
            cmd.CommandText = $"SELECT COUNT(*) FROM employees INNER JOIN persons ON employees.id=persons.id WHERE employees.bank_id='{bankId}' AND employees.id='{employeeId}' AND persons.is_active=TRUE;";
            if (Convert.ToInt32(cmd.ExecuteScalar()) <= 0)
            {
                conn.Close();
                throw new EmployeeDoesNotExistException();
            }
            conn.Close();
        }

        public void CheckAccountExistance(string bankId, string accountId)
        {
            conn.Open();
            cmd.CommandText = $"SELECT COUNT(*) FROM accounts INNER JOIN persons ON accounts.id=persons.id WHERE accounts.bank_id='{bankId}' AND accounts.id='{accountId}' AND persons.is_active=TRUE;";
            if (Convert.ToInt32(cmd.ExecuteScalar()) <= 0)
            {
                conn.Close();
                throw new AccountDoesNotExistException();
            }
            conn.Close();
        }

        public void ValidateBankName(string bankName)
        {
            conn.Open();
            cmd.CommandText = $"SELECT COUNT(*) FROM banks WHERE name='{bankName}' AND is_active=TRUE;";
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                conn.Close();
                throw new BankNameAlreadyExistsException();
            }
            conn.Close();
        }

        public void ValidateAccountUsername(string bankId, string username)
        {
            conn.Open();
            cmd.CommandText = $"SELECT COUNT(*) FROM accounts INNER JOIN persons ON accounts.id=persons.id WHERE accounts.bank_id='{bankId}' AND persons.username='{username}' AND persons.is_active=TRUE;";
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                conn.Close();
                throw new UsernameAlreadyExistsException();
            }
            conn.Close();
        }

        public void ValidateEmployeeUsername(string bankId, string username)
        {
            conn.Open();
            cmd.CommandText = $"SELECT COUNT(*) FROM employees INNER JOIN persons ON employees.id=persons.id WHERE employees.bank_id='{bankId}' AND persons.username='{username}' AND persons.is_active=TRUE;";
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                conn.Close();
                throw new UsernameAlreadyExistsException();
            }
            conn.Close();
        }

        public void ValidateCurrencyName(string bankId, string currencyName)
        {
            conn.Open();
            cmd.CommandText = $"SELECT COUNT(*) FROM currencies WHERE bank_id='{bankId}' AND name='{currencyName}';";
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                conn.Close();
                throw new CurrencyAlreadyExistsException();
            }
            conn.Close();
        }

        public Bank GetBankById(string bankId)
        {
            CheckBankExistance(bankId);
            conn.Open();
            cmd.CommandText = $"SELECT * FROM banks WHERE id='{bankId}' AND is_active=TRUE;";
            using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Bank bank = new Bank
                {
                    Id = rdr.GetString(0),
                    Name = rdr.GetString(1),
                    IMPS = rdr.GetDouble(2),
                    RTGS = rdr.GetDouble(3),
                    OIMPS = rdr.GetDouble(4),
                    ORTGS = rdr.GetDouble(5),
                };
                conn.Close();
                return bank;
            }
            conn.Close();
            return null;
        }

        public Employee GetEmployeeById(string bankId, string employeeId)
        {
            CheckEmployeeExistance(bankId, employeeId);
            conn.Open();
            cmd.CommandText = $"SELECT * FROM employees INNER JOIN persons ON employees.id=persons.id WHERE employees.bank_id='{bankId}' AND employees.id='{employeeId}' AND persons.is_active=TRUE;";
            using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Employee employee = new Employee
                {
                    Id = rdr.GetString(0),
                    BankId = rdr.GetString(1),
                    EmployeeType = (EmployeeType)rdr.GetInt32(2),
                    Name = rdr.GetString(4),
                    Gender = (Gender)rdr.GetInt32(5),
                    Username = rdr.GetString(6),
                    Password = rdr.GetString(7)
                };
                conn.Close();
                return employee;
            }
            conn.Close();
            return null;
        }

        public Account GetAccountById(string bankId, string accountId)
        {
            CheckAccountExistance(bankId, accountId);
            conn.Open();
            cmd.CommandText = $"SELECT * FROM accounts INNER JOIN persons ON accounts.id=persons.id WHERE accounts.bank_id='{bankId}' AND accounts.id='{accountId}' AND persons.is_active=TRUE;";
            using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Account account = new Account
                {
                    Id = rdr.GetString(0),
                    BankId = rdr.GetString(1),
                    AccountType = (AccountType)rdr.GetInt32(2),
                    Balance = rdr.GetDecimal(3),
                    Name = rdr.GetString(5),
                    Gender = (Gender)rdr.GetInt32(6),
                    Username = rdr.GetString(7),
                    Password = rdr.GetString(8)
                };
                conn.Close();
                return account;
            }
            conn.Close();
            return null;
        }

        public Currency GetCurrencyByName(string bankId, string currencyName)
        {
            CheckCurrencyExistance(bankId, currencyName);
            conn.Open();
            cmd.CommandText = $"SELECT * FROM currencies WHERE bank_id='{bankId}' AND name='{currencyName}';";
            using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Currency currency = new Currency
                {
                    BankId = rdr.GetString(1),
                    Name = rdr.GetString(2),
                    ExchangeRate = rdr.GetDouble(3)
                };
                conn.Close();
                return currency;
            }
            conn.Close();
            return null;
        }

        public Transaction GetTransactionById(string bankId, string txnId)
        {
            conn.Open();
            cmd.CommandText = $"SELECT * FROM transactions WHERE bank_id='{bankId}' AND id='{txnId}';";
            using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Transaction transaction = new Transaction
                {
                    Id = rdr.GetString(0),
                    AccountId = rdr.GetString(1),
                    BankId = rdr.GetString(2),
                    TransactionDate = rdr.GetDateTime(3),
                    TransactionType = (TransactionType)rdr.GetInt32(4),
                    ToBankId = Convert.ToString(rdr.GetValue(5)),
                    ToAccountId = Convert.ToString(rdr.GetValue(6)),
                    TransactionNarrative = (TransactionNarrative)rdr.GetInt32(7),
                    TransactionAmount = rdr.GetDecimal(8)
                };
                conn.Close();
                return transaction;
            }
            conn.Close();
            return null;
        }

        public IList<Transaction> GetTransactions(string bankId, string accountId)
        {
            IList<Transaction> transactions = new List<Transaction>();
            conn.Open();
            cmd.CommandText = $"SELECT * FROM transactions WHERE bank_id='{bankId}' AND account_id='{accountId}';";
            using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Transaction transaction = new Transaction
                {
                    Id = rdr.GetString(0),
                    AccountId = rdr.GetString(1),
                    BankId = rdr.GetString(2),
                    TransactionDate = rdr.GetDateTime(3),
                    TransactionType = (TransactionType)rdr.GetInt32(4),
                    ToBankId = Convert.ToString(rdr.GetValue(5)),
                    ToAccountId = Convert.ToString(rdr.GetValue(6)),
                    TransactionNarrative = (TransactionNarrative)rdr.GetInt32(7),
                    TransactionAmount = rdr.GetDecimal(8)
                };
                transactions.Add(transaction);
            }
            conn.Close();
            return transactions;
        }

        public IList<EmployeeAction> GetEmployeeActions(string bankId, string employeeId)
        {
            IList<EmployeeAction> actions = new List<EmployeeAction>();
            conn.Open();
            cmd.CommandText = $"SELECT * FROM actions WHERE bank_id='{bankId}' AND employee_id='{employeeId}';";
            using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                EmployeeAction action = new EmployeeAction
                {
                    Id = rdr.GetString(0),
                    BankId = rdr.GetString(1),
                    EmployeeId = rdr.GetString(2),
                    TXNId = Convert.ToString(rdr.GetValue(3)),
                    AccountId = Convert.ToString(rdr.GetValue(4)),
                    ActionDate = rdr.GetDateTime(5),
                    ActionType = (EmployeeActionType)rdr.GetInt32(6)
                };
                actions.Add(action);
            }
            conn.Close();
            return actions;
        }

        public void UpdateBank(Bank bank)
        {
            conn.Open();
            cmd.CommandText = $"UPDATE banks SET name='{bank.Name}',imps={bank.IMPS},rtgs={bank.RTGS},oimps={bank.OIMPS},ortgs={bank.ORTGS} WHERE id='{bank.Id}' AND is_active=TRUE;";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void UpdateEmployee(Employee employee)
        {
            conn.Open();
            cmd.CommandText = $"UPDATE employees INNER JOIN persons ON employees.id=persons.id SET employees.type={(int)employee.EmployeeType},persons.name='{employee.Name}',persons.gender={(int)employee.Gender},persons.username='{employee.Username}',persons.password='{employee.Password}' WHERE employees.id='{employee.Id}' AND employees.bank_id='{employee.BankId}' AND persons.is_active=TRUE;";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void UpdateAccount(Account account)
        {
            conn.Open();
            cmd.CommandText = $"UPDATE accounts INNER JOIN persons ON accounts.id=persons.id SET accounts.type={(int)account.AccountType},accounts.balance={account.Balance},persons.name='{account.Name}',persons.gender={(int)account.Gender},persons.username='{account.Username}',persons.password='{account.Password}' WHERE accounts.id='{account.Id}' AND accounts.bank_id='{account.BankId}' AND persons.is_active=TRUE;";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void UpdateCurrency(Currency currency)
        {
            conn.Open();
            cmd.CommandText = $"UPDATE currencies SET exchange_rate={currency.ExchangeRate} WHERE bank_id='{currency.BankId}' AND name='{currency.Name}';";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void DeleteBank(string bankId)
        {
            conn.Open();
            cmd.CommandText = $"UPDATE banks SET is_active=FALSE,deleted_on=NOW() WHERE id='{bankId}' AND is_active=TRUE;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"UPDATE employees INNER JOIN persons ON employees.id=persons.id SET persons.is_active=FALSE,persons.deleted_on=NOW() WHERE employees.bank_id='{bankId}';";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"UPDATE accounts INNER JOIN persons ON accounts.id=persons.id SET persons.is_active=FALSE,persons.deleted_on=NOW() WHERE accounts.bank_id='{bankId}';";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"DELETE FROM currencies WHERE bank_id='{bankId}';";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void DeletePerson(string id)
        {
            conn.Open();
            cmd.CommandText = $"Update persons SET is_active=FALSE,deleted_on=NOW() WHERE id='{id}';";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void DeleteCurrency(string bankId, string currencyName)
        {
            conn.Open();
            cmd.CommandText = $"DELETE FROM currencies WHERE bank_id='{bankId}' AND name='{currencyName}';";
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
