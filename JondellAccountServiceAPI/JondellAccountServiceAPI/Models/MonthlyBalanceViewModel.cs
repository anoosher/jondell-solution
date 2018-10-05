using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JondellAccountServiceAPI.Models
{
    public class MonthlyBalanceViewModel
    {
        public string DisplayMonth { get; set; }
        public int Order { get; set; }
        public List<MonthlyAccountBalanceViewModel> MonthlyBalanceViewModels { get; set; }
    }
}
