using ATM.Models.Enums;
using System;
using System.Collections.Generic;

namespace ATM.Models
{
    public class Account : Person
    {
        public string BankId { get; set; }
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; } = 1500;
    }
}
