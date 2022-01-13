using ATM.API.Models;
using ATM.Models;
using ATM.Services;
using ATM.Services.IServices;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//configuring mapper
IMapper mapper = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MapperProfile());
    mc.CreateMap<Bank, BankDTO>();
    mc.CreateMap<AccountCreateDTO, Account>().ForMember(dest => dest.Password, act => act.Ignore());
    mc.CreateMap<EmployeeCreateDTO, Employee>().ForMember(dest => dest.Password, act => act.Ignore());
}).CreateMapper();

builder.Services.AddDbContext<BankContext>(options => options.UseSqlServer(connectionString: @"Data Source=AKASH-VIVOBOOK\SQLEXPRESS03;Initial Catalog=Banking_Application;Integrated Security=true"))
    .AddSingleton(mapper)
    .AddSingleton<IEncryptionService, EncryptionService>()
    .AddScoped<ITransactionService, TransactionService>()
    .AddScoped<IEmployeeActionService, EmployeeActionService>()
    .AddScoped<ICurrencyService, CurrencyService>()
    .AddScoped<IAccountService, AccountService>()
    .AddScoped<IEmployeeService, EmployeeService>()
    .AddScoped<IBankService, BankService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
