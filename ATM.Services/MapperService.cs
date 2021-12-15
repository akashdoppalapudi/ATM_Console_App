using ATM.Models;
using ATM.Services.DBModels;
using ATM.Services.IServices;
using AutoMapper;

namespace ATM.Services
{
    public class MapperService : IMapperService
    {
        private readonly Mapper accountDBMapper, dbAccountMapper, bankDBMapper, dbBankMapper, employeeDBMapper, dbEmployeeMapper, currencyDBMapper, dbCurrencyMapper, employeeActionDBMapper, dbEmployeeActionMapper, transactionDBMapper, dbTransactionMapper;

        public MapperService()
        {
            accountDBMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Account, AccountDBModel>()));
            dbAccountMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<AccountDBModel, Account>()));
            bankDBMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Bank, BankDBModel>()));
            dbBankMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<BankDBModel, Bank>()));
            employeeDBMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDBModel>()));
            dbEmployeeMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<EmployeeDBModel, Employee>()));
            currencyDBMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Currency, CurrencyDBModel>()));
            dbCurrencyMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<CurrencyDBModel, Currency>()));
            employeeActionDBMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<EmployeeAction, EmployeeActionDBModel>()));
            dbEmployeeActionMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<EmployeeActionDBModel, EmployeeAction>()));
            transactionDBMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Transaction, TransactionDBModel>()));
            dbTransactionMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<TransactionDBModel, Transaction>()));
        }

        public AccountDBModel MapAccountToDB(Account account)
        {
            return accountDBMapper.Map<AccountDBModel>(account);
        }

        public Account MapDBToAccount(AccountDBModel accountDBModel)
        {
            return dbAccountMapper.Map<Account>(accountDBModel);
        }

        public BankDBModel MapBankToDB(Bank bank)
        {
            return bankDBMapper.Map<BankDBModel>(bank);
        }

        public Bank MapDBToBank(BankDBModel bankModel)
        {
            return dbBankMapper.Map<Bank>(bankModel);
        }

        public EmployeeDBModel MapEmployeeToDB(Employee employee)
        {
            return employeeDBMapper.Map<EmployeeDBModel>(employee);
        }

        public Employee MapDBToEmployee(EmployeeDBModel employeeDbModel)
        {
            return dbEmployeeMapper.Map<Employee>(employeeDbModel);
        }

        public CurrencyDBModel MapCurrencyToDB(Currency currency)
        {
            return currencyDBMapper.Map<CurrencyDBModel>(currency);
        }

        public Currency MapDBToCurrency(CurrencyDBModel currencyDBModel)
        {
            return dbCurrencyMapper.Map<Currency>(currencyDBModel);
        }

        public EmployeeActionDBModel MapEmployeeActionToDB(EmployeeAction employeeAction)
        {
            return employeeActionDBMapper.Map<EmployeeActionDBModel>(employeeAction);
        }

        public EmployeeAction MapDBToEmployeeAction(EmployeeActionDBModel employeeActionDBModel)
        {
            return dbEmployeeActionMapper.Map<EmployeeAction>(employeeActionDBModel);
        }

        public TransactionDBModel MapTransctionToDB(Transaction transaction)
        {
            return transactionDBMapper.Map<TransactionDBModel>(transaction);
        }

        public Transaction MapDBToTransaction(TransactionDBModel transactionDBModel)
        {
            return dbTransactionMapper.Map<Transaction>(transactionDBModel);
        }
    }
}
