using ATM.Models;
using AutoMapper;
using ATM.Services.DBModels;
using System.Linq;
using ATM.Services.Exceptions;
using ATM.Services.IServices;

namespace ATM.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly MapperConfiguration currencyDBConfig;
        private readonly Mapper currencyDBMapper;
        private readonly MapperConfiguration dbCurrencyConfig;
        private readonly Mapper dbCurrencyMapper;

        public CurrencyService()
        {
            currencyDBConfig = new MapperConfiguration(cfg => cfg.CreateMap<Currency, CurrencyDBModel>());
            currencyDBMapper = new Mapper(currencyDBConfig);
            dbCurrencyConfig = new MapperConfiguration(cfg => cfg.CreateMap<CurrencyDBModel, Currency>());
            dbCurrencyMapper = new Mapper(dbCurrencyConfig);
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

        public Currency GetCurrencyByName(string bankId, string currencyName)
        {
            CheckCurrencyExistance(bankId, currencyName);
            using (BankContext bankContext = new BankContext())
            {
                CurrencyDBModel currencyRecord = bankContext.Currency.FirstOrDefault(c => c.BankId == bankId && c.Name == currencyName);
                return dbCurrencyMapper.Map<Currency>(currencyRecord);
            }
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
            CurrencyDBModel currencyRecord = currencyDBMapper.Map<CurrencyDBModel>(currency);
            using (BankContext bankContext = new BankContext())
            {
                bankContext.Currency.Add(currencyRecord);
                bankContext.SaveChanges();
            }
        }

        public void UpdateCurrency(string bankId, string currencyName, Currency updateCurrency)
        {
            using (BankContext bankContext = new BankContext())
            {
                CurrencyDBModel currentCurrencyRecord = bankContext.Currency.First(c => c.BankId == bankId && c.Name == currencyName);
                currentCurrencyRecord = currencyDBMapper.Map<CurrencyDBModel>(updateCurrency);
                bankContext.SaveChanges();
            }
        }

        public void DeleteCurrency(string bankId, string currencyName)
        {
            CheckCurrencyExistance(bankId, currencyName);
            using (BankContext bankContext = new BankContext())
            {
                bankContext.Remove(bankContext.Currency.First(c => c.BankId == bankId && c.Name == currencyName));
                bankContext.SaveChanges();
            }
        }
    }
}
