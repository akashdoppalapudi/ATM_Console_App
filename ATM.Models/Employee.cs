using ATM.Models.Enums;
using System;

namespace ATM.Models
{
    [Serializable]
    public class Employee
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public string Password { get; set; }
    }
}
