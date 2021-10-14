using System;
using System.Collections.Generic;
using ATM.Models.Enums;

namespace ATM.Models
{
    [Serializable]
    public class Account
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public AccountType AccountType { get; set; }
        public string Pin { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
