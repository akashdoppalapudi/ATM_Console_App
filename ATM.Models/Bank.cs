using System;
using System.Collections.Generic;

namespace ATM.Models
{
    [Serializable]
    public class Bank
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public List<Account> Accounts { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Currency> Currencies { get; set; }
        public double IMPS { get; set; }
        public double RTGS { get; set; }
        public double OIMPS { get; set; }
        public double ORTGS { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
