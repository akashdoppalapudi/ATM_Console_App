using ATM.Models;
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
        private readonly IIDGenService _idGenService;
        private readonly MapperConfiguration transactionDBConfig;
        private readonly Mapper transactionDBMapper;
        private readonly MapperConfiguration dbTransactionConfig;
        private readonly Mapper dbTransactionMapper;
        private readonly BankContext _bankContext;

        public TransactionService(IIDGenService idGenService, BankContext bankContext)
        {
            _idGenService = idGenService;
            transactionDBConfig = new MapperConfiguration(cfg => cfg.CreateMap<Transaction, TransactionDBModel>());
            transactionDBMapper = new Mapper(transactionDBConfig);
            dbTransactionConfig = new MapperConfiguration(cfg => cfg.CreateMap<TransactionDBModel, Transaction>());
            dbTransactionMapper = new Mapper(dbTransactionConfig);
            _bankContext = bankContext;
        }

        public Transaction CreateTransaction(string bankId, string accountId, decimal amount, TransactionType transactionType, TransactionNarrative transactionNarrative, string fromAccId, string toBankId = null, string toAccId = null)
        {
            Transaction newTransaction = new Transaction
            {
                Id = _idGenService.GenTransactionId(bankId, accountId),
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
            TransactionDBModel transactionRecord = transactionDBMapper.Map<TransactionDBModel>(transaction);
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
            return dbTransactionMapper.Map<Transaction>(transactionRecord);
        }

        public IList<Transaction> GetTransactions(string bankId, string accountId)
        {
            IList<Transaction> transactions;
            transactions = dbTransactionMapper.Map<Transaction[]>(_bankContext.Transaction.Where(t => t.BankId == bankId && t.AccountId == accountId).ToList());
            if (transactions.Count == 0 || transactions == null)
            {
                throw new NoTransactionsException();
            }
            return transactions;
        }
    }
}
