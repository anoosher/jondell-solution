using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jondell.AccountService.Data
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<MonthlyBalance> MonthlyBalanceList { get; set; }
        
    }
}
