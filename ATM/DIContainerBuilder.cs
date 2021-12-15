using Microsoft.Extensions.DependencyInjection;
using ATM.Services;
using ATM.Services.IServices;
using System;

namespace ATM.CLI
{
    public class DIContainerBuilder
    {
        public IServiceProvider Build()
        {
            ServiceCollection DIContainer = new ServiceCollection();

            DIContainer
                .AddSingleton<IIDGenService, IDGenService>()
                .AddSingleton<IEncryptionService, EncryptionService>()
                .AddSingleton<ITransactionService, TransactionService>()
                .AddSingleton<IEmployeeActionService, EmployeeActionService>()
                .AddSingleton<ICurrencyService, CurrencyService>()
                .AddSingleton<IAccountService, AccountService>()
                .AddSingleton<IEmployeeService, EmployeeService>()
                .AddSingleton<IBankService, BankService>()
                .AddSingleton<IConsoleMessages, ConsoleMessages>()
                .AddSingleton<IConsoleUI, ConsoleUI>();

            return DIContainer.BuildServiceProvider();
        }
    }
}
