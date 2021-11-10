using ATM.Models.Enums;
using System;

namespace ATM.Models
{
    [Serializable]
    public class EmployeeAction
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string TXNId { get; set; }
        public string AccountId { get; set; }
        public DateTime ActionDate { get; set; }
        public EmployeeActionType ActionType { get; set; }
    }
}
