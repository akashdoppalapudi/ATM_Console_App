using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ATM.Models
{
    public class Bank
    {
        [StringLength(30)]
        [Required]
        public string Name { get; set; }
        [Required]
        [Key]
        [StringLength(20)]
        public string Id { get; set; }
        [Required]
        public double IMPS { get; set; } = 5;
        [Required]
        public double RTGS { get; set; } = 0;
        [Required]
        public double OIMPS { get; set; } = 6;
        [Required]
        public double ORTGS { get; set; } = 2;
        [Required]
        public bool IsActive { get; set; } = true;
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? DeletedOn { get; set; } = null;
        public virtual IList<Currency> Currencies { get; set; }
        public virtual IList<Account> Accounts { get; set; }
        public virtual IList<Employee> Employees { get; set; }
        public virtual IList<Transaction> Transactions { get; set; }
        public virtual IList<EmployeeAction> EmployeeActions { get; set; }
    }
}
