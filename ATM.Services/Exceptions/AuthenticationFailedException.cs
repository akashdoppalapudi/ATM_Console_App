using System;

namespace ATM.Services.Exceptions
{
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException() : base("Wrong Password. Authentication Failed") { }
    }
}
