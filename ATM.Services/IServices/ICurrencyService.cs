using ATM.Models;
using System.Collections.Generic;

namespace ATM.Services.IServices
{
    public interface ICurrencyService
    {
        void AddCurrency(Currency currency);
        void DeleteCurrency(string bankId, string currencyName);
        Currency GetCurrencyByName(string bankId, string currencyName);
        IList<Currency> GetAllCurrencies(string bankId);
        void UpdateCurrency(string bankId, string currencyName, Currency updateCurrency);
        bool IsCurrencyNameExists(string bankId, string currencyName);
    }
}