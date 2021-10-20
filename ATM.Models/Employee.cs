using ATM.Models.Enums;
using System;

namespace ATM.Models
{
    [Serializable]
    public class Employee: Person
    {
        public EmployeeType EmployeeType { get; set; }
    }
}
