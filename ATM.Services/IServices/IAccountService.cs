using ATM.Models;
using ATM.Models.ViewModels;

namespace ATM.Services.IServices
{
    public interface IAccountService
    {
        void AddAccount(Account account);
        void Authenticate(string accountId, string password);
        void DeleteAccount(string accountId);
        void Deposit(string accountId, Currency currency, decimal amount);
        AccountViewModel GetAccountDetails(string accountId);
        string GetAccountIdByUsername(string bankId, string username);
        decimal GetBalance(string accountId);
        void Transfer(string selectedAccountId, string transferToAccountId, decimal amount);
        void UpdateAccount(string accountId, Account updateAccount);
        bool IsUsernameExists(string bankId, string username);
        void Withdraw(string accountId, decimal amount);
    }
}