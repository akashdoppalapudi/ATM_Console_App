using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.Exceptions;

namespace ATM.Services
{
    public class CurrencyService
    {
        private IList<Currency> currencies;
        private readonly DataService dataService;

        public CurrencyService()
        {
            dataService = new DataService();
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
    }
}
