using System;
using System.Collections.Generic;

namespace ATM.Models
{
    [Serializable]
    public class Bank
    {
        public string Name { get; }
        public string Address { get; }
        public List<Account> Accounts { get; set; }
    }
}
