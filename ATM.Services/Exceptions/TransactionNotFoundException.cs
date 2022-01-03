using System;

namespace ATM.Services.Exceptions
{
    public class TransactionNotFoundException : Exception
    {
        public TransactionNotFoundException() : base("Transaction not found") { }
    }
}
