using System;

namespace ATM.Models
{
    public class Transaction
    {
        public DateTime timeStamp { get; set; }
        public TransactionType transactionType { get; set; }
        public decimal transactionAmount { get; set; }
    }
}
