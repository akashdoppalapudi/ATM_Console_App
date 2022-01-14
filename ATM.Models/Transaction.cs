using ATM.Models.Enums;
using System;

namespace ATM.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public TransactionType TransactionType { get; set; }
        public string ToBankId { get; set; }
        public string ToAccountId { get; set; }
        public TransactionNarrative TransactionNarrative { get; set; }
        public decimal TransactionAmount { get; set; }
        public string BankId { get; set; }
        public string AccountId { get; set; }
    }
}
