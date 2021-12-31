using ATM.Models;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using ATM.Services.IServices;
using AutoMapper;
using System.Linq;

namespace ATM.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IMapper _mapper;
        private readonly BankContext _bankContext;

        public CurrencyService(BankContext bankContext, IMapper mapper)
        {
            _mapper = mapper;
            _bankContext = bankContext;
        }

        public bool IsCurrencyNameExists(string bankId, string currencyName)
        {
            return _bankContext.Currency.Any(c => c.BankId == bankId && c.Name == currencyName);
        }

        public Currency GetCurrencyByName(string bankId, string currencyName)
        {
            CurrencyDBModel currencyRecord = _bankContext.Currency.FirstOrDefault(c => c.BankId == bankId && c.Name == currencyName);
            if (currencyRecord == null)
            {
                throw new CurrencyDoesNotExistException();
            }
            return _mapper.Map<Currency>(currencyRecord);
        }

        public void AddCurrency(Currency currency)
        {
            CurrencyDBModel currencyRecord = _mapper.Map<CurrencyDBModel>(currency);
            _bankContext.Currency.Add(currencyRecord);
            _bankContext.SaveChanges();
        }

        public void UpdateCurrency(string bankId, string currencyName, Currency updateCurrency)
        {
            CurrencyDBModel currentCurrencyRecord = _bankContext.Currency.FirstOrDefault(c => c.BankId == bankId && c.Name == currencyName);
            if (currentCurrencyRecord == null)
            {
                throw new CurrencyDoesNotExistException();
            }
            currentCurrencyRecord.ExchangeRate = updateCurrency.ExchangeRate;
            _bankContext.SaveChanges();
        }

        public void DeleteCurrency(string bankId, string currencyName)
        {
            CurrencyDBModel currency = _bankContext.Currency.FirstOrDefault(c => c.BankId == bankId && c.Name == currencyName);
            if (currency == null)
            {
                throw new CurrencyDoesNotExistException();
            }
            _bankContext.Remove(currency);
            _bankContext.SaveChanges();
        }
    }
}
