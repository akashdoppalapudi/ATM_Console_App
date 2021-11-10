using System;
using System.Collections.Generic;

namespace ATM.Models
{
    public class Bank
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public double IMPS { get; set; } = 5;
        public double RTGS { get; set; } = 0;
        public double OIMPS { get; set; } = 6;
        public double ORTGS { get; set; } = 2;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime? DeletedOn { get; set; } = null;
    }
}
