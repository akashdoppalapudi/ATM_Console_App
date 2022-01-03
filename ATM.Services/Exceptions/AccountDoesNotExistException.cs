using System;

namespace ATM.Services.Exceptions
{
    public class AccountDoesNotExistException : Exception
    {
        public AccountDoesNotExistException() : base("User Not Found") { }
    }
}
