using ATM.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ATM.Models
{
    public class Person
    {
        [Key]
        [Required]
        [StringLength(20)]
        public string Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        [StringLength(20)]
        public string Username { get; set; }
        [Required]
        [MaxLength(64)]
        public byte[] Password { get; set; }
        [Required]
        [MaxLength(24)]
        public byte[] Salt { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? DeletedOn { get; set; } = null;
    }
}
