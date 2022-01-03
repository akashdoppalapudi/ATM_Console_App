using System;

namespace ATM.Services.Exceptions
{
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException() : base("An account with that username already exists") { }
    }
}
