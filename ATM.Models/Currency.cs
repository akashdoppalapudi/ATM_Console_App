using System.ComponentModel.DataAnnotations;

namespace ATM.Models
{
    public class Currency
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
        public Bank Bank { get; set; }
    }
}
