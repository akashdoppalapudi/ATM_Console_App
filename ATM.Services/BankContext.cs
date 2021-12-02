using ATM.Models;
using Microsoft.EntityFrameworkCore;

namespace ATM.Services
{
    internal class BankContext : DbContext
    {
        public DbSet<Bank> Bank { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<EmployeeAction> EmployeeAction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString: @"server=localhost;database=test;user=root;password=Akash@1729");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasOne(d => d.Bank)
                .WithMany(p => p.Currencies)
                .HasForeignKey(d => d.BankId);
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasOne(d => d.Bank)
                .WithMany(p => p.Accounts)
                .HasForeignKey(d => d.BankId);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(d => d.Bank)
                .WithMany(p => p.Employees)
                .HasForeignKey(d => d.BankId);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasOne(d => d.Account)
                .WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountId);
                entity.HasOne(d => d.Bank)
                .WithMany(p => p.Transactions)
                .HasForeignKey(d => d.BankId);
            });

            modelBuilder.Entity<EmployeeAction>(entity =>
            {
                entity.HasOne(d => d.Employee)
                .WithMany(p => p.EmployeeActions)
                .HasForeignKey(d => d.EmployeeId);
                entity.HasOne(d => d.Bank)
                .WithMany(p => p.EmployeeActions)
                .HasForeignKey(d => d.BankId);
            });
        }
    }
}
