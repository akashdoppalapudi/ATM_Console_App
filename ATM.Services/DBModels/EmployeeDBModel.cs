using ATM.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ATM.Services.DBModels
{
    public class EmployeeDBModel : PersonDBModel
    {
        [Required]
        public EmployeeType EmployeeType { get; set; }
        [Required]
        [StringLength(20)]
        public string BankId { get; set; }
        public BankDBModel Bank { get; set; }
        public virtual IList<EmployeeActionDBModel> EmployeeActions { get; set; }
    }
}
