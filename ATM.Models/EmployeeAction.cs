using ATM.Models.Enums;
using System;

namespace ATM.Models
{
    public class EmployeeAction
    {
        public string Id { get; set; }
        public string TXNId { get; set; }
        public string AccountId { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public EmployeeActionType ActionType { get; set; }
        public string EmployeeId { get; set; }
        public string BankId { get; set; }
    }
}
