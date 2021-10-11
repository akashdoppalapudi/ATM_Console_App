using System;

namespace ATM.Models
{
    [Serializable]
    public class Transaction
    {
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
