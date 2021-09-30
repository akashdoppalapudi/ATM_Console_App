using ATM.Models;
using System;

namespace ATM.Services
{
    public class TransactionHandler
    {
        public static Transaction accountCreationTransaction()
        {
            Transaction newTransaction = new Transaction
            {
                timeStamp = DateTime.Now,
                transactionAmount = 1500,
                transactionType = (TransactionType)1
            };

            return newTransaction;
        }

        public static Transaction depositTransaction(decimal amount)
        {
            Transaction newTransaction = new Transaction
            {
                timeStamp = DateTime.Now,
                transactionAmount = amount,
                transactionType = (TransactionType)3
            };

            return newTransaction;
        }
    }
}
