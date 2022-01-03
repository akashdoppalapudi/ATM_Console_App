using System;

namespace ATM.Services.Exceptions
{
    public class BankNameAlreadyExistsException : Exception
    {
        public BankNameAlreadyExistsException() : base("Bank with that name already exists") { }
    }
}
