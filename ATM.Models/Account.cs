using ATM.Models.Enums;

namespace ATM.Models
{
    public class Account : Person
    {
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; } = 1500;
        public string BankId { get; set; }
    }
}
