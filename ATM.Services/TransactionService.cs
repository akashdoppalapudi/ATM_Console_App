using ATM.Models;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using ATM.Services.IServices;
using AutoMapper;
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

        public void AddTransaction(Transaction transaction)
        {
            TransactionDBModel transactionRecord = _mapper.Map<TransactionDBModel>(transaction);
            _bankContext.Transaction.Add(transactionRecord);
            _bankContext.SaveChanges();
        }

        public Transaction GetTransactionById(string txnId)
        {
            TransactionDBModel transactionRecord = _bankContext.Transaction.FirstOrDefault(t => t.Id == txnId);
            if (transactionRecord == null)
            {
                throw new TransactionNotFoundException();
            }
            return _mapper.Map<Transaction>(transactionRecord);
        }

        public IList<Transaction> GetTransactions(string accountId)
        {
            IList<Transaction> transactions;
            IList<TransactionDBModel> transactionRecords = _bankContext.Transaction.Where(t => t.AccountId == accountId).ToList();
            if (transactionRecords.Count == 0 || transactionRecords == null)
            {
                throw new NoTransactionsException();
            }
            transactions = _mapper.Map<Transaction[]>(transactionRecords);
            return transactions;
        }
    }
}
