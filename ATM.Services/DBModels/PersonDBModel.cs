using ATM.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ATM.Services.DBModels
{
    public class PersonDBModel
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
        public bool IsActive { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
