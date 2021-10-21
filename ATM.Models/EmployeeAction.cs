using System;
using ATM.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Models
{
    public class EmployeeAction
    {
        public string Id { get; set; }
        public string TXNId { get; set; }
        public string AccountId { get; set; }
        public DateTime ActionDate { get; set; }
        public EmployeeActionType ActionType { get; set; }
    }
}
