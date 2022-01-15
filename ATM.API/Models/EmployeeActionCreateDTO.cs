using ATM.Models.Enums;

namespace ATM.API.Models
{
    public class EmployeeActionCreateDTO
    {
        public string? TXNId { get; set; }
        public string? AccountId { get; set; }
        public EmployeeActionType ActionType { get; set; }
        public string BankId { get; set; }
        public string EmployeeId { get; set; }
    }
}
