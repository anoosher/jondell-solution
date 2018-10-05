using System;
using System.ComponentModel.DataAnnotations;

namespace Jondell.AccountService.Data
{
    public class MonthlyBalance
    {
        [Key]
        public Guid Id { get; set; }
        public string Month { get; set; }
        public Account Account { get; set; }
        public string Balance { get; set; }
        public bool IsLatest { get; set; }
        public DateTime AddedTime { get; set; }
        public string AddedBy { get; set; }
        public int Order { get; set; }
    }
}