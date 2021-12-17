using ATM.Services;
using ATM.Services.IServices;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ATM.CLI
{
    public class DIContainerBuilder
    {
        public IServiceProvider Build()
        {
            // follow the naming conventions when nameing a variable all time
            ServiceCollection services = new ServiceCollection();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperService());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services
                .AddSingleton<IEncryptionService, EncryptionService>()
                //.AddSingleton<IMapperService, MapperService>()
                .AddSingleton<ITransactionService, TransactionService>()
                .AddSingleton<IEmployeeActionService, EmployeeActionService>()
                .AddSingleton<ICurrencyService, CurrencyService>()
                .AddSingleton<IAccountService, AccountService>()
                .AddSingleton<IEmployeeService, EmployeeService>()
                .AddSingleton<IBankService, BankService>()
                .AddSingleton<IConsoleMessages, ConsoleMessages>()
                .AddSingleton<IConsoleUI, ConsoleUI>()
                .AddDbContext<BankContext>();

            return services.BuildServiceProvider();
        }
    }
}