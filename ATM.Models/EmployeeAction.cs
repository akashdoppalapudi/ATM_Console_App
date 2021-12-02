using ATM.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ATM.Models
{
    [Serializable]
    public class EmployeeAction
    {
        [Key]
        [Required]
        [StringLength(55)]
        public string Id { get; set; }
        [StringLength(55)]
        public string TXNId { get; set; }
        [StringLength(20)]
        public string AccountId { get; set; }
        [Required]
        public DateTime ActionDate { get; set; }
        [Required]
        public EmployeeActionType ActionType { get; set; }
        [Required]
        [StringLength(20)]
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Required]
        [StringLength(20)]
        public string BankId { get; set; }
        public virtual Bank Bank { get; set; }
    }
}
