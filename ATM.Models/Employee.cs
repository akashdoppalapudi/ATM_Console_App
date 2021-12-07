using ATM.Models.Enums;

namespace ATM.Models
{
    public class Employee : Person
    {
        public EmployeeType EmployeeType { get; set; }
        public string BankId { get; set; }
    }
}
