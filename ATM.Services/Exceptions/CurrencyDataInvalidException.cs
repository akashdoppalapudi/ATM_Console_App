using System;

namespace ATM.Services.Exceptions
{
    public class CurrencyDataInvalidException : Exception
    {
        public CurrencyDataInvalidException() : base("Invalid Data for Currency") { }
    }
}
