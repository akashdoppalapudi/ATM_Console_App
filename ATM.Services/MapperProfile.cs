using ATM.Models;
using ATM.Models.ViewModels;
using ATM.Services.DBModels;
using AutoMapper;

namespace ATM.Services
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Account, AccountDBModel>();
            CreateMap<AccountDBModel, Account>();
            CreateMap<Bank, BankDBModel>();
            CreateMap<BankDBModel, Bank>();
            CreateMap<Employee, EmployeeDBModel>();
            CreateMap<EmployeeDBModel, Employee>();
            CreateMap<Currency, CurrencyDBModel>();
            CreateMap<CurrencyDBModel, Currency>();
            CreateMap<EmployeeAction, EmployeeActionDBModel>();
            CreateMap<EmployeeActionDBModel, EmployeeAction>();
            CreateMap<Transaction, TransactionDBModel>();
            CreateMap<TransactionDBModel, Transaction>();
            CreateMap<AccountDBModel, AccountViewModel>();
            CreateMap<EmployeeDBModel, EmployeeViewModel>();
        }
    }
}
