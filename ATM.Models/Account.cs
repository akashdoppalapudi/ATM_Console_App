using ATM.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ATM.Models
{
    public class Account : Person
    {
        [Required]
        public AccountType AccountType { get; set; }
        [Required]
        public decimal Balance { get; set; } = 1500;
        [Required]
        [StringLength(20)]
        public string BankId { get; set; }
        public Bank Bank { get; set; }
        public virtual IList<Transaction> Transactions { get; set; }
    }
}
