using System.Collections.Generic;

namespace ATM.Models
{
    public class Account
    {
        public int accountNumber { get; set; }
        public string accountHoldersName { get; set; }
        public AccountType accountType { get; set; }
        public int pin { get; set; }
        public decimal availableBalance { get; set; }
        public List<Transaction> transactions { get; set; }
    }
}
