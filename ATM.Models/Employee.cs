using ATM.Models.Enums;

namespace ATM.Models
{
    public class Employee : Person
    {
        public string BankId { get; set; }
        public EmployeeType EmployeeType { get; set; }
    }
}
