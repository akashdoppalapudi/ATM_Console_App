using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ATM.Services.DBModels
{
    public class BankDBModel
    {
        [StringLength(30)]
        [Required]
        public string Name { get; set; }
        [Required]
        [Key]
        [StringLength(20)]
        public string Id { get; set; }
        [Required]
        public double IMPS { get; set; }
        [Required]
        public double RTGS { get; set; }
        [Required]
        public double OIMPS { get; set; }
        [Required]
        public double ORTGS { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public virtual IList<CurrencyDBModel> Currencies { get; set; }
        public virtual IList<AccountDBModel> Accounts { get; set; }
        public virtual IList<EmployeeDBModel> Employees { get; set; }
        public virtual IList<TransactionDBModel> Transactions { get; set; }
        public virtual IList<EmployeeActionDBModel> EmployeeActions { get; set; }
    }
}
