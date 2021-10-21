using System;

namespace ATM.Services
{
    public class IDGenService
    {
        public string GenBankId(string bankName)
        {
            string bankId;
            DateTime date = DateTime.Today;
            string dateStr = date.ToString().Substring(0, 10).Replace("-", string.Empty);
            bankId = bankName.Substring(0, 3).ToUpper() + dateStr;
            return bankId;
        }

        public string GenAccountId(string accName)
        {
            string AccountId;
            DateTime date = DateTime.Today;
            string dateStr = date.ToString().Substring(0, 10).Replace("-", string.Empty);
            AccountId = accName.Substring(0, 3).ToUpper() + dateStr;
            return AccountId;
        }

        public string GenEmployeeId(string empName)
        {
            string EmployeeId = GenAccountId(empName) + 'S';
            return EmployeeId;
        }

        public string GenTransactionId(string bankId, string accId)
        {
            string TXNId;
            DateTime date = DateTime.Today;
            string dateStr = date.ToString().Substring(0, 10).Replace("-", string.Empty);
            TXNId = "TXN" + bankId + accId + dateStr;
            return TXNId;
        }
    }
}
