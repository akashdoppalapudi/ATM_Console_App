using System;

namespace ATM.Services.Exceptions
{
    public class BankCreationFailedException : Exception
    {
        public BankCreationFailedException() : base("Bank Creation Failed") { }
    }
}
