using ATM.Models;

namespace ATM.Services
{
    public class CurrencyService
    {
        private readonly DBService dbService;

        public CurrencyService()
        {
            dbService = new DBService();
        }

        public void CheckCurrencyExistance(string bankId, string currencyName)
        {
            dbService.CheckCurrencyExistance(bankId, currencyName);
        }

        public void ValidateCurrencyName(string bankId, string currencyName)
        {
            dbService.ValidateCurrencyName(bankId, currencyName);
        }

        public Currency GetCurrencyByName(string bankId, string currencyName)
        {
            return dbService.GetCurrencyByName(bankId, currencyName);
        }

        public Currency CreateCurrency(string currencyName, double exchangeRate)
        {
            Currency newCurrency = new Currency
            {
                Name = currencyName,
                ExchangeRate = exchangeRate,
            };
            return newCurrency;
        }

        public void AddCurrency(string bankId, Currency currency)
        {
            currency.BankId = bankId;
            dbService.AddCurrency(currency);
        }

        public void UpdateCurrency(string bankId, string currencyName, Currency updateCurrency)
        {
            Currency currency = GetCurrencyByName(bankId, currencyName);
            currency.ExchangeRate = updateCurrency.ExchangeRate;
            dbService.UpdateCurrency(currency);
        }

        public void DeleteCurrency(string bankId, string currencyName)
        {
            CheckCurrencyExistance(bankId, currencyName);
            dbService.DeleteCurrency(bankId, currencyName);
        }
    }
}
