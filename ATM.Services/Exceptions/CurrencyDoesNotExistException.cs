using System;

namespace ATM.Services.Exceptions
{
    public class CurrencyDoesNotExistException : Exception
    {
        public CurrencyDoesNotExistException() : base("Currency with that name does not exist") { }
    }
}
