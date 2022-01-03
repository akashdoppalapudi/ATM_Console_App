using System;

namespace ATM.Services.Exceptions
{
    public class NoTransactionsException : Exception
    {
        public NoTransactionsException() : base("There are no transactions for this account") { }
    }
}
