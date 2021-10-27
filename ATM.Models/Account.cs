using ATM.Models.Enums;
using System;
using System.Collections.Generic;

namespace ATM.Models
{
    [Serializable]
    public class Account : Person
    {
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
