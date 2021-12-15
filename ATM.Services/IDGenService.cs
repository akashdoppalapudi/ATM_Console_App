using System;

namespace ATM.Services
{
    public static class IDGenService
    {
        public static string GenId(this string Name)
        {
            string Id;
            Id = Name.Substring(0, 3).ToUpper() + GetDateStr();
            return Id;
        }

        public static string GenTransactionId(this string bankId, string accId)
        {
            string TXNId;
            TXNId = "TXN" + bankId + accId + GetDateStr();
            return TXNId;
        }

        public static string GenEmployeeActionId(this string bankId, string empId)
        {
            string ACNId;
            ACNId = "ACN" + bankId + empId + GetDateStr();
            return ACNId;
        }

        private static string GetDateStr()
        {
            DateTime date = DateTime.Now;
            string dateStr = date.ToString().Replace("-", string.Empty).Replace(" ", string.Empty).Replace(":", string.Empty);
            return dateStr;
        }
    }
}
