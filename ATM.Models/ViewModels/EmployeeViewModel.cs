using ATM.Models.Enums;

namespace ATM.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public string Username { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public string BankId { get; set; }
    }
}
