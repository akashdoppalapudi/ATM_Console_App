namespace ATM.API.Models
{
    public class BankDTO
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public double IMPS { get; set; }
        public double RTGS { get; set; }
        public double OIMPS { get; set; }
        public double ORTGS { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
