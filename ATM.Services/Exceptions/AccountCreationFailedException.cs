using System;

namespace ATM.Services.Exceptions
{
    public class AccountCreationFailedException : Exception
    {
        public AccountCreationFailedException() : base("Account Creation Failed") { }
    }
}
