namespace ATM.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double ExchangeRate { get; set; }
        public string BankId { get; set; }
    }
}
