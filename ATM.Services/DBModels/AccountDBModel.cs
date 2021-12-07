using ATM.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ATM.Services.DBModels
{
    public class AccountDBModel : PersonDBModel
    {
        [Required]
        public AccountType AccountType { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        [StringLength(20)]
        public string BankId { get; set; }
        public BankDBModel Bank { get; set; }
        public virtual IList<TransactionDBModel> Transactions { get; set; }
    }
}
