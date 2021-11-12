using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class TransactionService
    {
        private IList<Transaction> transactions;
        private readonly DataService dataService;
        private readonly IDGenService idGenService;

        public TransactionService()
        {
            dataService = new DataService();
            idGenService = new IDGenService();
            PopulateTransactionData();
        }

        private void PopulateTransactionData()
        {
            this.transactions = dataService.ReadTransactionData();
            if (this.transactions == null)
            {
                this.transactions = new List<Transaction>();
            }
        }

        public Transaction CreateTransaction(string bankId, string accountId, decimal amount, TransactionType transactionType, TransactionNarrative transactionNarrative, string fromAccId, string toBankId = null, string toAccId = null)
        {
            Transaction newTransaction = new Transaction
            {
                Id = idGenService.GenTransactionId(bankId, accountId),
                TransactionDate = DateTime.Now,
                TransactionType = transactionType,
                FromAccountId = fromAccId,
                ToBankId = toBankId,
                ToAccountId = toAccId,
                TransactionNarrative = transactionNarrative,
                TransactionAmount = amount
            };
            return newTransaction;
        }

        public void AddTransaction(string bankId, string accountId, Transaction transaction)
        {
            PopulateTransactionData();
            transaction.AccountId = accountId;
            transaction.BankId = bankId;
            this.transactions.Add(transaction);
            dataService.WriteTransactionData(this.transactions);
        }

        public Transaction GetTransactionById(string bankId, string txnId)
        {
            PopulateTransactionData();
            IList<Transaction> possibleTransactions = this.transactions.Where(t => t.Id == txnId && t.BankId == bankId && t.TransactionType == TransactionType.Debit && t.TransactionNarrative == TransactionNarrative.Transfer).ToList();
            if (possibleTransactions.Count <= 0)
            {
                throw new TransactionNotFoundException();
            }
            return possibleTransactions.OrderBy(t => t.TransactionDate).Last();
        }

        public IList<Transaction> GetTransactions(string bankId, string accountId)
        {
            PopulateTransactionData();
            return this.transactions.Where(t => t.AccountId == accountId && t.BankId == bankId).ToList();
        }
    }
}
