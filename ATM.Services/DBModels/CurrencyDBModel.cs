using System.ComponentModel.DataAnnotations;

namespace ATM.Services.DBModels
{
    public class CurrencyDBModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [StringLength(3)]
        [Required]
        public string Name { get; set; }
        [Required]
        public double ExchangeRate { get; set; }
        [Required]
        [StringLength(20)]
        public string BankId { get; set; }
        public BankDBModel Bank { get; set; }
    }
}
