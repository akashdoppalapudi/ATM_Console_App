using ATM.Services.IServices;
using System;

namespace ATM.Services
{
    public class IDGenService : IIDGenService
    {
        public string GenId(string Name)
        {
            string Id;
            Id = Name.Substring(0, 3).ToUpper() + GetDateStr();
            return Id;
        }

        public string GenTransactionId(string bankId, string accId)
        {
            string TXNId;
            TXNId = "TXN" + bankId + accId + GetDateStr();
            return TXNId;
        }

        public string GenEmployeeActionId(string bankId, string empId)
        {
            string ACNId;
            ACNId = "ACN" + bankId + empId + GetDateStr();
            return ACNId;
        }

        private string GetDateStr()
        {
            DateTime date = DateTime.Now;
            string dateStr = date.ToString().Replace("-", string.Empty).Replace(" ", string.Empty).Replace(":", string.Empty);
            return dateStr;
        }
    }
}
