using ATM.Models;
using ATM.Services.DBModels;
using ATM.Services.IServices;
using AutoMapper;

namespace ATM.Services
{
    // no need of imapper service
    // should inherit Automapper.Profile class
    public class MapperService : Profile
    {
        private readonly Mapper mapper;

        public MapperService()
        {
            CreateMap<Account, AccountDBModel>();
            mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Account, AccountDBModel>();
                cfg.CreateMap<AccountDBModel, Account>();
                cfg.CreateMap<Bank, BankDBModel>();
                cfg.CreateMap<BankDBModel, Bank>();
                cfg.CreateMap<Employee, EmployeeDBModel>();
                cfg.CreateMap<EmployeeDBModel, Employee>();
                cfg.CreateMap<Currency, CurrencyDBModel>();
                cfg.CreateMap<CurrencyDBModel, Currency>();
                cfg.CreateMap<EmployeeAction, EmployeeActionDBModel>();
                cfg.CreateMap<EmployeeActionDBModel, EmployeeAction>();
                cfg.CreateMap<Transaction, TransactionDBModel>();
                cfg.CreateMap<TransactionDBModel, Transaction>();
            }));
        }

        // We dont need this to be done separately do this in service layer itself, remove all the the code which is in 'Not Required' region

        #region Not Required
        public AccountDBModel MapAccountToDB(Account account)
        {
            return mapper.Map<AccountDBModel>(account);
        }

        public Account MapDBToAccount(AccountDBModel accountDBModel)
        {
            return mapper.Map<Account>(accountDBModel);
        }

        public BankDBModel MapBankToDB(Bank bank)
        {
            return mapper.Map<BankDBModel>(bank);
        }

        public Bank MapDBToBank(BankDBModel bankModel)
        {
            return mapper.Map<Bank>(bankModel);
        }

        public EmployeeDBModel MapEmployeeToDB(Employee employee)
        {
            return mapper.Map<EmployeeDBModel>(employee);
        }

        public Employee MapDBToEmployee(EmployeeDBModel employeeDbModel)
        {
            return mapper.Map<Employee>(employeeDbModel);
        }

        public CurrencyDBModel MapCurrencyToDB(Currency currency)
        {
            return mapper.Map<CurrencyDBModel>(currency);
        }

        public Currency MapDBToCurrency(CurrencyDBModel currencyDBModel)
        {
            return mapper.Map<Currency>(currencyDBModel);
        }

        public EmployeeActionDBModel MapEmployeeActionToDB(EmployeeAction employeeAction)
        {
            return mapper.Map<EmployeeActionDBModel>(employeeAction);
        }

        public EmployeeAction MapDBToEmployeeAction(EmployeeActionDBModel employeeActionDBModel)
        {
            return mapper.Map<EmployeeAction>(employeeActionDBModel);
        }

        public TransactionDBModel MapTransctionToDB(Transaction transaction)
        {
            return mapper.Map<TransactionDBModel>(transaction);
        }

        public Transaction MapDBToTransaction(TransactionDBModel transactionDBModel)
        {
            return mapper.Map<Transaction>(transactionDBModel);
        }

        #endregion
    }
}
