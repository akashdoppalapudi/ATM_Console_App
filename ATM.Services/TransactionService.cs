using ATM.Models;
using ATM.Models.Enums;
using System;
using System.Collections.Generic;

namespace ATM.Services
{
    public class TransactionService
    {
        private readonly IDGenService idGenService;
        private readonly DBService dbService;

        public TransactionService()
        {
            idGenService = new IDGenService();
            dbService = new DBService();
        }

        public Transaction CreateTransaction(string bankId, string accountId, decimal amount, TransactionType transactionType, TransactionNarrative transactionNarrative, string fromAccId, string toBankId = null, string toAccId = null)
        {
            Transaction newTransaction = new Transaction
            {
                Id = idGenService.GenTransactionId(bankId, accountId),
                TransactionDate = DateTime.Now,
                TransactionType = transactionType,
                BankId = bankId,
                AccountId = fromAccId,
                ToBankId = toBankId,
                ToAccountId = toAccId,
                TransactionNarrative = transactionNarrative,
                TransactionAmount = amount
            };
            return newTransaction;
        }

        public void AddTransaction(string bankId, string accountId, Transaction transaction)
        {
            transaction.AccountId = accountId;
            transaction.BankId = bankId;
            dbService.AddTransaction(transaction);
        }

        public Transaction GetTransactionById(string bankId, string txnId)
        {
            return dbService.GetTransactionById(bankId, txnId);
        }

        public IList<Transaction> GetTransactions(string bankId, string accountId)
        {
            return dbService.GetTransactions(bankId, accountId);
        }
    }
}
