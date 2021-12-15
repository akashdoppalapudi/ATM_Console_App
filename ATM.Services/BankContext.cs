using ATM.Services.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ATM.Services
{
    public class BankContext : DbContext
    {
        public DbSet<BankDBModel> Bank { get; set; }
        public DbSet<CurrencyDBModel> Currency { get; set; }
        public DbSet<AccountDBModel> Account { get; set; }
        public DbSet<EmployeeDBModel> Employee { get; set; }
        public DbSet<TransactionDBModel> Transaction { get; set; }
        public DbSet<EmployeeActionDBModel> EmployeeAction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString: @"Data Source=AKASH-VIVOBOOK\SQLEXPRESS03;Initial Catalog=Banking_Application;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
