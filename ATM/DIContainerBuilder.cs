using ATM.Services;
using ATM.Services.IServices;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ATM.CLI
{
    public class DIContainerBuilder
    {
        public IServiceProvider Build()
        {
            ServiceCollection DIContainer = new ServiceCollection();

            DIContainer
                .AddSingleton<IEncryptionService, EncryptionService>()
                .AddSingleton<IMapperService, MapperService>()
                .AddSingleton<ITransactionService, TransactionService>()
                .AddSingleton<IEmployeeActionService, EmployeeActionService>()
                .AddSingleton<ICurrencyService, CurrencyService>()
                .AddSingleton<IAccountService, AccountService>()
                .AddSingleton<IEmployeeService, EmployeeService>()
                .AddSingleton<IBankService, BankService>()
                .AddSingleton<IConsoleMessages, ConsoleMessages>()
                .AddSingleton<IConsoleUI, ConsoleUI>()
                .AddDbContext<BankContext>();

            return DIContainer.BuildServiceProvider();
        }
    }
}
