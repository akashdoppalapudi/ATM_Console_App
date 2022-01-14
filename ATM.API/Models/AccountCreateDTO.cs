using ATM.Models.Enums;

namespace ATM.API.Models
{
    public class AccountCreateDTO
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public string Username { get; set; }
        public AccountType AccountType { get; set; }
        public string Password { get; set; }
        public string BankId { get; set; }
    }
}
