using ATM.Models;
using System.Collections.Generic;

namespace ATM.Services.IServices
{
    public interface ITransactionService
    {
        void AddTransaction(Transaction transaction);
        Transaction GetTransactionById(string txnId);
        IList<Transaction> GetTransactions(string accountId);
    }
}