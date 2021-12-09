using ATM.Models;
using System.Collections.Generic;

namespace ATM.Services.IServices
{
    public interface IBankService
    {
        void AddAccount(string bankId, string employeeId, Account newAccount);
        void AddBank(Bank bank, Employee adminEmployee);
        void AddCurrency(string bankId, string employeeId, Currency currency);
        void AddEmployee(string bankId, string employeeId, Employee newEmployee);
        void CheckBankExistance(string bankId);
        Bank CreateBank(string name);
        void DeleteAccount(string bankId, string employeeId, string accountId);
        void DeleteBank(string bankId, string employeeId);
        void DeleteCurrency(string bankId, string employeeId, string currencyName);
        void DeleteEmployee(string bankId, string employeeId, string deleteEmployeeId);
        void Deposit(string bankId, string accountId, Currency currency, decimal amount);
        Dictionary<string, string> GetAllBankNames();
        Bank GetBankById(string bankId);
        Bank GetBankDetails(string bankId);
        void RevertTransaction(string bankId, string employeeId, string txnId);
        void Transfer(string selectedBankId, string selectedAccountId, string transferToBankId, string transferToAccountId, decimal amount);
        void UpdateAccount(string bankId, string employeeId, string currentAccountId, Account updateAccount);
        void UpdateBank(string bankId, string employeeId, Bank updateBank);
        void UpdateCurrency(string bankId, string employeeId, string currencyName, Currency updateCurrency);
        void UpdateEmployee(string bankId, string employeeId, string currentEmployeeId, Employee updateEmployee);
        void ValidateBankName(string bankName);
        void Withdraw(string bankId, string accountId, decimal amount);
    }
}