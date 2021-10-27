using System;

namespace ATM.Models
{
    [Serializable]
    public class Currency
    {
        public string Name { get; set; }
        public double ExchangeRate { get; set; }
    }
}
