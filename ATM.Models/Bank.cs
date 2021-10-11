using System;
using System.Collections.Generic;

namespace ATM.Models
{
    [Serializable]
    public class Bank
    {
        public string Name { get; set;  }
        public string Address { get; set;  }
        public List<Account> Accounts { get; set; }
    }
}
