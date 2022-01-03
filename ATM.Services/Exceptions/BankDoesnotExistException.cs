using System;

namespace ATM.Services.Exceptions
{
    public class BankDoesnotExistException : Exception
    {
        public BankDoesnotExistException() : base("Bank Does not Exist") { }
    }
}
