using ATM.Models;
using ATM.Models.Enums;
using System;

namespace ATM.Services
{
    public class TransactionHandler
    {
        public Transaction NewTransaction(string TXNID, decimal amount, TransactionType transactionType)
        {
            Transaction newTransaction = new Transaction
            {
                Id = TXNID,
                TransactionDate = DateTime.Now,
                TransactionAmount = amount,
                TransactionType = transactionType
            };

            return newTransaction;
        }
    }
}
