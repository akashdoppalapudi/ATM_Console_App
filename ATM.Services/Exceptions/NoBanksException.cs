using System;

namespace ATM.Services.Exceptions
{
    public class NoBanksException : Exception
    {
        public NoBanksException() : base("Currently there are no banks.\nTry Creating a new bank.") { }
    }
}
