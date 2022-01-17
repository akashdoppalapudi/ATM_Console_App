using ATM.Models;
using System.Collections.Generic;

namespace ATM.Services.IServices
{
    public interface IBankService
    {
        void AddBank(Bank bank);
        void DeleteBank(string bankId);
        IList<Bank> GetAllBanks();
        Bank GetBankDetails(string bankId);
        void RevertTransaction(string txnId);
        void UpdateBank(string bankId, Bank updateBank);
        bool IsBankNameExists(string bankName);
    }
}