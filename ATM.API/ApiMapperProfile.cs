using ATM.API.Models;
using ATM.Models;
using AutoMapper;

namespace ATM.API
{
    public class ApiMapperProfile : Profile
    {
        public ApiMapperProfile()
        {
            CreateMap<Bank, BankDTO>();
            CreateMap<AccountCreateDTO, Account>().ForMember(dest => dest.Password, act => act.Ignore());
            CreateMap<EmployeeCreateDTO, Employee>().ForMember(dest => dest.Password, act => act.Ignore());
            CreateMap<EmployeeActionCreateDTO, EmployeeAction>();
            CreateMap<TransactionCreateDTO, Transaction>();
        }
    }
}
