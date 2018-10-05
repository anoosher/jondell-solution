using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Jondell.AccountService.Data
{
    public class DBAccountDataService : IAccountDataService
    {
        AccountServiceDbContext dbContext;

        public DBAccountDataService(AccountServiceDbContext accountServiceDbContext)
        {
            this.dbContext = accountServiceDbContext;
        }

        public void AddAccount(Account account)
        {
            dbContext.Add(account);
            dbContext.SaveChanges();
        }

        public void AddAccounts(List<MonthlyBalance> monthlyAccBalancesToInsert)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    //getting latest account balances to make them not latest
                    IEnumerable<MonthlyBalance> latestRecordsInTheDB = this.GetLatestAccountBalances();

                    foreach (MonthlyBalance monthlyBalanceToUpdate in latestRecordsInTheDB)
                    {
                        monthlyBalanceToUpdate.IsLatest = false;
                        dbContext.Update(monthlyBalanceToUpdate);
                    }

                    dbContext.SaveChanges();

                    //inserting new latest records
                    foreach (MonthlyBalance monthlyBalanceToInsert in monthlyAccBalancesToInsert)
                    {
                        dbContext.Add(monthlyBalanceToInsert);
                    }

                    dbContext.SaveChanges();

                    transaction.Commit();

                }
                catch (Exception e)
                {
                    throw;
                }

                
            }
        }

        public void AddMonthlyBalance(MonthlyBalance monthlyBalance)
        {
            dbContext.Add(monthlyBalance);
            dbContext.SaveChanges();
        }

        public IEnumerable<Account> GetAccounts()
        {
            return dbContext.Accounts;
        }

        public Account GetAccountsByName(string displayAccountName)
        {
            Account selectedAccount = dbContext.Accounts.Where(mb => mb.Name == displayAccountName).FirstOrDefault();
            return selectedAccount;
        }

        public IEnumerable<MonthlyBalance> GetLatestAccountBalances()
        {

            IEnumerable<MonthlyBalance>  latestBalances = dbContext.MonthlyBalances.Include(mb=>mb.Account).Where(mb => mb.IsLatest)
                                      .ToList();
            
            return latestBalances;
        }

        public IEnumerable<MonthlyBalance> GetMonthlyBalances()
        {
            IEnumerable<MonthlyBalance> balances = dbContext.MonthlyBalances.Include(mb => mb.Account)
                                      .ToList();

            return balances;
        }
    }
}
