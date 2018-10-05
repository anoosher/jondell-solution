using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jondell.AccountService.Data
{
    public class AccountServiceDbContext : IdentityDbContext<IdentityUser>
    {
        public AccountServiceDbContext(DbContextOptions<AccountServiceDbContext> options)
        : base(options)
        { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<MonthlyBalance> MonthlyBalances { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().HasData(new Account { Id = Guid.NewGuid(), MonthlyBalanceList= null, Name= "R&D" });
            modelBuilder.Entity<Account>().HasData(new Account { Id = Guid.NewGuid(), MonthlyBalanceList = null, Name = "Canteen" });
            modelBuilder.Entity<Account>().HasData(new Account
            {
                Id = Guid.NewGuid(),
                MonthlyBalanceList = null,
                Name = "CEO’s car"
            });
            modelBuilder.Entity<Account>().HasData(new Account
            {
                Id = Guid.NewGuid(),
                MonthlyBalanceList = null,
                Name = "Marketing"
            });
            modelBuilder.Entity<Account>().HasData(new Account
            {
                Id = Guid.NewGuid(),
                MonthlyBalanceList = null,
                Name = "Parking fines"
            });
        }
    }
}
