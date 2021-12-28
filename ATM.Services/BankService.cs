﻿using ATM.Models;
using ATM.Services.DBModels;
using ATM.Services.Exceptions;
using ATM.Services.IServices;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Services
{
    public class BankService : IBankService
    {
        private readonly ITransactionService _transactionService;
        private readonly IEmployeeService _employeeService;
        private readonly IAccountService _accountService;
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;
        private readonly BankContext _bankContext;

        public BankService(ITransactionService transactionService, IEmployeeService employeeService, IAccountService accountService, ICurrencyService currencyService, BankContext bankContext, IMapper mapper)
        {
            _transactionService = transactionService;
            _employeeService = employeeService;
            _accountService = accountService;
            _currencyService = currencyService;
            _mapper = mapper;
            _bankContext = bankContext;
            _bankContext.Database.EnsureCreated();
        }

        public void AddBank(Bank bank)
        {
            BankDBModel bankRecord = _mapper.Map<BankDBModel>(bank);
            _bankContext.Bank.Add(bankRecord);
            _bankContext.SaveChanges();
        }

        public void UpdateBank(string bankId, Bank updateBank)
        {
            BankDBModel currentBankRecord = _bankContext.Bank.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            if (currentBankRecord == null)
            {
                throw new BankDoesnotExistException();
            }
            currentBankRecord.Name = updateBank.Name;
            currentBankRecord.IMPS = updateBank.IMPS;
            currentBankRecord.RTGS = updateBank.RTGS;
            currentBankRecord.OIMPS = updateBank.OIMPS;
            currentBankRecord.ORTGS = updateBank.ORTGS;
            _bankContext.SaveChanges();
        }

        public void DeleteBank(string bankId)
        {
            BankDBModel bankRecord = _bankContext.Bank.First(b => b.Id == bankId && b.IsActive);
            bankRecord.IsActive = false;
            bankRecord.DeletedOn = DateTime.Now;
            var employeeRecords = _bankContext.Employee.Where(e => e.BankId == bankId && e.IsActive).ToList();
            employeeRecords.ForEach(e => e.IsActive = false);
            var accountRecords = _bankContext.Account.Where(a => a.BankId == bankId && a.IsActive).ToList();
            accountRecords.ForEach(a => a.IsActive = false);
            _bankContext.Currency.RemoveRange(_bankContext.Currency.Where(c => c.BankId == bankId));
            _bankContext.SaveChanges();
        }

        public Dictionary<string, string> GetAllBankNames()
        {
            Dictionary<string, string> bankNames = new Dictionary<string, string>();
            var bankRecords = _bankContext.Bank.Where(b => b.IsActive).Select(b => new { b.Id, b.Name });
            foreach (var bankRecord in bankRecords)
            {
                bankNames.Add(bankRecord.Id, bankRecord.Name);
            }
            return bankNames;
        }

        public void RevertTransaction(string bankId, string txnId)
        {
            Transaction transaction = _transactionService.GetTransactionById(txnId);
            decimal amount = transaction.TransactionAmount;
            string fromAccId = transaction.AccountId;
            string toAccId = transaction.ToAccountId;
            string toBankId = transaction.ToBankId;
            _accountService.Transfer(toAccId, fromAccId, amount);
        }

        public void ValidateBankName(string bankName)
        {
            if (_bankContext.Bank.Any(b => b.Name == bankName && b.IsActive))
            {
                throw new BankNameAlreadyExistsException();
            }
        }

        public Bank GetBankDetails(string bankId)
        {
            BankDBModel bankRecord = _bankContext.Bank.FirstOrDefault(b => b.Id == bankId && b.IsActive);
            if (bankRecord == null)
            {
                throw new BankDoesnotExistException();
            }
            return _mapper.Map<Bank>(bankRecord);
        }
    }
}
