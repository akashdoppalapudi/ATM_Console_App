using ATM.Models;
using System;

namespace ATM.Services
{
    public class TransactionHandler
    {
        public static Transaction NewTransaction(decimal amount, TransactionType transactionType)
        {
            Transaction newTransaction = new Transaction
            {
                TransactionDate = DateTime.Now,
                TransactionAmount = amount,
                TransactionType = transactionType
            };

            return newTransaction;
        }
    }
}
