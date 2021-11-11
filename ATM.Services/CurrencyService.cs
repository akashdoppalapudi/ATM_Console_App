using ATM.Models;
using ATM.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class CurrencyService
    {
        private IList<Currency> currencies;
        private readonly DataService dataService;
        private readonly BankService bankService;

        public CurrencyService()
        {
            dataService = new DataService();
            bankService = new BankService();
            PopulateCurrencyData();
        }

        private void PopulateCurrencyData()
        {
            this.currencies = dataService.ReadCurrencyData();
            if (this.currencies == null)
            {
                this.currencies = new List<Currency>();
            }
        }

        public void CheckCurrencyExistance(string bankId, string currencyName)
        {
            try
            {
                bankService.CheckBankExistance(bankId);
                PopulateCurrencyData();
                if (this.currencies.Any(c => c.BankId == bankId && c.Name == currencyName))
                {
                    return;
                }
                throw new CurrencyDoesNotExistException();
            }
            catch (BankDoesnotExistException)
            {
                throw new CurrencyDoesNotExistException();
            }
        }

        public Currency GetCurrencyByName(string bankId, string currencyName)
        {
            CheckCurrencyExistance(bankId, currencyName);
            return this.currencies.FirstOrDefault(c => c.BankId == bankId && c.Name == currencyName);
        }

        public Currency CreateCurrency(string currencyName, double exchangeRate)
        {
            PopulateCurrencyData();
            Currency newCurrency = new Currency
            {
                Id = this.currencies.Count + 1,
                Name = currencyName,
                ExchangeRate = exchangeRate,
            };
            return newCurrency;
        }

        public void AddCurrency(string bankId, Currency currency)
        {
            PopulateCurrencyData();
            currency.BankId = bankId;
            this.currencies.Add(currency);
            dataService.WriteCurrencyData(this.currencies);
        }

        public void UpdateCurrency(string bankId, string currencyName, Currency updateCurrency)
        {
            PopulateCurrencyData();
            Currency currency = GetCurrencyByName(bankId, currencyName);
            currency.ExchangeRate = updateCurrency.ExchangeRate;
            dataService.WriteCurrencyData(this.currencies);
        }

        public void DeleteCurrency(string bankId, string currencyName)
        {
            PopulateCurrencyData();
            this.currencies.Remove(GetCurrencyByName(bankId, currencyName));
            dataService.WriteCurrencyData(this.currencies);
        }
    }
}
