using System;
using System.Collections.Generic;

namespace Jondell.AccountService.Data
{
    public interface IAccountDataService
    {

        IEnumerable<Account> GetAccounts();
        void AddMonthlyBalance(MonthlyBalance monthlyBalance);
        void AddAccount(Account account);
        IEnumerable<MonthlyBalance> GetMonthlyBalances();
        IEnumerable<MonthlyBalance> GetLatestAccountBalances();
        Account GetAccountsByName(string displayAccountName);
        void AddAccounts(List<MonthlyBalance> monthlyAccBalancesToInsert);
    }
}
