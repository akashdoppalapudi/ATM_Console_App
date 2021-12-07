using ATM.Models;

namespace ATM.Services.IServices
{
    public interface ICurrencyService
    {
        void AddCurrency(string bankId, Currency currency);
        void CheckCurrencyExistance(string bankId, string currencyName);
        Currency CreateCurrency(string currencyName, double exchangeRate);
        void DeleteCurrency(string bankId, string currencyName);
        Currency GetCurrencyByName(string bankId, string currencyName);
        void UpdateCurrency(string bankId, string currencyName, Currency updateCurrency);
        void ValidateCurrencyName(string bankId, string currencyName);
    }
}