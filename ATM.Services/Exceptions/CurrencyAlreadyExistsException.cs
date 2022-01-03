using System;

namespace ATM.Services.Exceptions
{
    public class CurrencyAlreadyExistsException : Exception
    {
        public CurrencyAlreadyExistsException() : base("Currency Already Exists") { }
    }
}
