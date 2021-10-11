using System;
using System.Collections.Generic;

namespace ATM.Models
{
    [Serializable]
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountHoldersName { get; set; }
        public AccountType AccountType { get; set; }
        public string Pin { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
