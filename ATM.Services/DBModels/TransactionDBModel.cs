using ATM.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ATM.Services.DBModels
{
    public class TransactionDBModel
    {
        [Key]
        [Required]
        [StringLength(55)]
        public string Id { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        [StringLength(20)]
        public string ToBankId { get; set; }
        [StringLength(20)]
        public string ToAccountId { get; set; }
        [Required]
        public TransactionNarrative TransactionNarrative { get; set; }
        [Required]
        public decimal TransactionAmount { get; set; }
        [Required]
        [StringLength(20)]
        public string BankId { get; set; }
        public BankDBModel Bank { get; set; }
        [Required]
        [StringLength(20)]
        public string AccountId { get; set; }
        public AccountDBModel Account { get; set; }
    }
}
