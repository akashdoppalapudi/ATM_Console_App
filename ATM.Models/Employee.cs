using ATM.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ATM.Models
{
    public class Employee : Person
    {
        [Required]
        public EmployeeType EmployeeType { get; set; }
        [Required]
        [StringLength(20)]
        public string BankId { get; set; }
        public Bank Bank { get; set; }
        public virtual IList<EmployeeAction> EmployeeActions { get; set; }
    }
}
