using ATM.Models;

namespace ATM.Services.IServices
{
    public interface ICurrencyService
    {
        void AddCurrency(Currency currency);
        void DeleteCurrency(string bankId, string currencyName);
        Currency GetCurrencyByName(string bankId, string currencyName);
        void UpdateCurrency(string bankId, string currencyName, Currency updateCurrency);
        void ValidateCurrencyName(string bankId, string currencyName);
    }
}