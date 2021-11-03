using ATM.Models.Enums;
using System;
using System.Collections.Generic;

namespace ATM.Models
{
    [Serializable]
    public class Employee : Person
    {
        public EmployeeType EmployeeType { get; set; }
        public List<EmployeeAction> EmployeeActions { get; set; } = new List<EmployeeAction>();
    }
}
