using System.Collections.Generic;

namespace ATM.Models
{
    public class Bank
    {
        public string name { get; } = "Alpha Bank";
        public string address { get; } = "Alpha Bank, Madhapur, Hyderabad.";
        public List<Account> accounts { get; set; }
    }
}
