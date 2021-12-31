using ATM.Models.Enums;

namespace ATM.Models.ViewModels
{
    public class AccountViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public string Username { get; set; }
        public AccountType AccountType { get; set; }
        public string BankId { get; set; }
    }
}
