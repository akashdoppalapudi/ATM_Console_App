using ATM.Models;
using ATM.Models.Enums;
using System;
using System.Collections.Generic;

namespace ATM.Services
{
    public class TransactionService
    {
        private IList<Transaction> transactions;
        private readonly DataService dataService;

        public TransactionService()
        {
            dataService = new DataService();
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
        public Transaction NewTransaction(string TXNID, decimal amount, TransactionType transactionType, TransactionNarrative transactionNarrative, string fromAccId, string toBankId = null, string toAccId = null)
        {
            Transaction newTransaction = new Transaction
            {
                Id = TXNID,
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

        public EmployeeAction NewEmployeeAction(string ACNID, EmployeeActionType actionType, string accId = null, string TXNID = null)
        {
            EmployeeAction newEmployeeAction = new EmployeeAction
            {
                Id = ACNID,
                TXNId = TXNID,
                AccountId = accId,
                ActionDate = DateTime.Now,
                ActionType = actionType
            };

            return newEmployeeAction;
        }
    }
}
