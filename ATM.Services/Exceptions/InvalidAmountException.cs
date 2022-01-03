using System;

namespace ATM.Services.Exceptions
{
    public class InvalidAmountException : Exception
    {
        public InvalidAmountException() : base("Invalid Amount") { }
    }
}
