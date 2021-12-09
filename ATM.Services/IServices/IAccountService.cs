using ATM.Models;
using ATM.Models.Enums;

namespace ATM.Services.IServices
{
    public interface IAccountService
    {
        void AddAccount(string bankId, Account account);
        void Authenticate(string bankId, string accountId, string password);
        void CheckAccountExistance(string bankId, string accountId);
        Account CreateAccount(string name, Gender gender, string username, string password, AccountType accountType);
        void DeleteAccount(string bankId, string accountId);
        void Deposit(string bankId, string accountId, Currency currency, decimal amount);
        Account GetAccountDetails(string bankId, string accountId);
        string GetAccountIdByUsername(string bankId, string username);
        decimal GetBalance(string bankId, string accountId);
        void Transfer(string selectedBankId, string selectedAccountId, string transferToBankId, string transferToAccountId, decimal amount);
        void UpdateAccount(string bankId, string accountId, Account updateAccount);
        void ValidateUsername(string bankId, string username);
        void Withdraw(string bankId, string accountId, decimal amount);
    }
}