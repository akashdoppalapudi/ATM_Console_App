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
                timeStamp = DateTime.Now,
                transactionAmount = amount,
                transactionType = transactionType
            };

            return newTransaction;
        }
    }
}
