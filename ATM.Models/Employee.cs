using ATM.Models.Enums;
using System;
using System.Collections.Generic;

namespace ATM.Models
{
    public class Employee : Person
    {
        public string BankId { get; set; }
        public EmployeeType EmployeeType { get; set; }
    }
}
