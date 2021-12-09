using ATM.Models;
using ATM.Models.Enums;
using System.Collections.Generic;

namespace ATM.Services.IServices
{
    public interface ITransactionService
    {
        void AddTransaction(string bankId, string accountId, Transaction transaction);
        Transaction CreateTransaction(string bankId, string accountId, decimal amount, TransactionType transactionType, TransactionNarrative transactionNarrative, string fromAccId, string toBankId = null, string toAccId = null);
        Transaction GetTransactionById(string bankId, string txnId);
        IList<Transaction> GetTransactions(string bankId, string accountId);
    }
}