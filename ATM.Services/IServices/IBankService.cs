using ATM.Models;
using System.Collections.Generic;

namespace ATM.Services.IServices
{
    public interface IBankService
    {
        void AddBank(Bank bank);
        void DeleteBank(string bankId);
        Dictionary<string, string> GetAllBankNames();
        Bank GetBankDetails(string bankId);
        void RevertTransaction(string bankId, string txnId);
        void UpdateBank(string bankId, Bank updateBank);
        void ValidateBankName(string bankName);
    }
}