using ATM.Models.Enums;
using System.Collections.Generic;
using System;

namespace ATM.Models
{
    [Serializable]
    public class Employee: Person
    {
        public EmployeeType EmployeeType { get; set; }
        public List<EmployeeAction> EmployeeActions { get; set; }
    }
}
