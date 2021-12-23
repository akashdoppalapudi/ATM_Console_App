﻿using ATM.Models;
using ATM.Models.Enums;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using ATM.Services.IServices;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly BankContext _bankContext;

        public TransactionService(BankContext bankContext, IMapper mapper)
        {
            _mapper = mapper;
            _bankContext = bankContext;
        }

        public Transaction CreateTransaction(string bankId, string accountId, decimal amount, TransactionType transactionType, TransactionNarrative transactionNarrative, string fromAccId, string toBankId = null, string toAccId = null)
        {
            Transaction newTransaction = new Transaction
            {
                Id = bankId.GenTransactionId(accountId),
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
            TransactionDBModel transactionRecord = _mapper.Map<TransactionDBModel>(transaction);
            _bankContext.Transaction.Add(transactionRecord);
            _bankContext.SaveChanges();
        }

        public Transaction GetTransactionById(string bankId, string txnId)
        {
            TransactionDBModel transactionRecord = _bankContext.Transaction.FirstOrDefault(t => t.BankId == bankId && t.Id == txnId);
            if (transactionRecord == null)
            {
                throw new TransactionNotFoundException();
            }
            return _mapper.Map<Transaction>(transactionRecord);
        }

        public IList<Transaction> GetTransactions(string bankId, string accountId)
        {
            IList<Transaction> transactions = new List<Transaction>();
            IList<TransactionDBModel> transactionRecords = _bankContext.Transaction.Where(t => t.BankId == bankId && t.AccountId == accountId).ToList();
            foreach (TransactionDBModel tdb in transactionRecords)
            {
                transactions.Add(_mapper.Map<Transaction>(tdb));
            }
            if (transactions.Count == 0 || transactions == null)
            {
                throw new NoTransactionsException();
            }
            return transactions;
        }
    }
}
