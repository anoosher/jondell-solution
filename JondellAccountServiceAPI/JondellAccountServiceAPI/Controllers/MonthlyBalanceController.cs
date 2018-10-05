using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Jondell.AccountService.Data;
using JondellAccountServiceAPI.Models;
using JondellAccountServiceAPI.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JondellAccountServiceAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/monthlyBalance")]
    public class MonthlyBalanceController : Controller
    {
        private readonly IAccountDataService accountDataService;

        public MonthlyBalanceController(IAccountDataService accountDataService)
        {
            this.accountDataService = accountDataService;
        }

        // GET: api/MonthlyBalance
        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ("Admin"))]
        public IActionResult Get()
        {
            try
            {
                List<MonthlyBalanceViewModel> monthlyBalanceList = new List<MonthlyBalanceViewModel>();

                List<MonthlyBalance> monthlyBalances = accountDataService.GetMonthlyBalances().ToList();

                if (monthlyBalances.Count > 0)
                {
                    var groupedBalances = monthlyBalances.GroupBy<MonthlyBalance, int>(mb => mb.Order);

                    foreach (var groupedBalanceSet in groupedBalances)
                    {
                        MonthlyBalanceViewModel mothlyBalanceToAdd = new MonthlyBalanceViewModel()
                        {
                            DisplayMonth = groupedBalanceSet.First().Month,
                            Order = groupedBalanceSet.First().Order,
                            MonthlyBalanceViewModels = new List<MonthlyAccountBalanceViewModel>()
                        };

                        foreach (var accountMonthlyBalance in groupedBalanceSet)
                        {
                            MonthlyAccountBalanceViewModel monthlyAccountBalanceViewModel = new MonthlyAccountBalanceViewModel()
                            {
                                DisplayAccountBalance = accountMonthlyBalance.Balance,
                                DisplayAccountName = accountMonthlyBalance.Account.Name
                            };

                            mothlyBalanceToAdd.MonthlyBalanceViewModels.Add(monthlyAccountBalanceViewModel);

                        }

                        monthlyBalanceList.Add(mothlyBalanceToAdd);
                    }

                }

                return Ok(monthlyBalanceList.OrderBy(mb=>mb.Order));

            }

                

            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        // GET: api/MonthlyBalance
        [HttpGet, Route("getLatestBalances"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles =("User"))] 
        public IActionResult GetLatestMonthlyBalances()
        {
            try
            {
                MonthlyBalanceViewModel viewModelToReturn = new MonthlyBalanceViewModel();

                List<MonthlyBalance> latestMonthlyBalances = accountDataService.GetLatestAccountBalances().ToList();

                if (latestMonthlyBalances.Count > 0)
                {
                    viewModelToReturn.DisplayMonth = latestMonthlyBalances.First().Month;
                    viewModelToReturn.MonthlyBalanceViewModels = new List<MonthlyAccountBalanceViewModel>();

                    foreach (var accMonthlyBalnace in latestMonthlyBalances)
                    {
                        MonthlyAccountBalanceViewModel monthlyAccountBalanceViewModel = new MonthlyAccountBalanceViewModel()
                        {
                            DisplayAccountBalance = accMonthlyBalnace.Balance,
                            DisplayAccountName = accMonthlyBalnace.Account.Name
                        };

                        viewModelToReturn.MonthlyBalanceViewModels.Add(monthlyAccountBalanceViewModel);
                    }

                }
                return Ok(viewModelToReturn);
                
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        // GET: api/MonthlyBalance/5
        [HttpGet("{id}", Name = "GetMonthlyBalance")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MonthlyBalance
        [HttpPost, Route("upload"), DisableRequestSizeLimit, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ("Admin"))]
        public void Upload()
        {
            var file = Request.Form.Files[0];

            // Get content of your Excel file
            var ms = new MemoryStream();

            file.CopyTo(ms);


            //Excel File Read
            try
            {

                string heading = ExcelReader.GetCellValue(ms, "Sheet1", "A1");

                string[] headingChunks = heading.Split(' ');
                string year = headingChunks.Last();
                int lastTwoIndex = headingChunks.Count() - 2;
                string month = headingChunks[lastTwoIndex];
                string iMonthNo = Convert.ToDateTime("01-" + month + "-" + year).Month.ToString("D2");
                string idString = year + iMonthNo;
                int orderNo = Int32.Parse(idString);

                List<MonthlyBalance> monthlyAccBalancesToInsert = new List<MonthlyBalance>();

                //Account R&D

                MonthlyBalance monthlyBalanceRDToInsert = new MonthlyBalance();
                monthlyBalanceRDToInsert.Id = Guid.NewGuid();
                monthlyBalanceRDToInsert.Account = accountDataService.GetAccountsByName(ExcelReader.GetCellValue(ms, "Sheet1", "A3"));
                monthlyBalanceRDToInsert.AddedTime = DateTime.Now;
                monthlyBalanceRDToInsert.Balance = ExcelReader.GetCellValue(ms, "Sheet1", "B3");
                monthlyBalanceRDToInsert.IsLatest = true;
                monthlyBalanceRDToInsert.Month = month + " " + year;
                monthlyBalanceRDToInsert.Order = orderNo;
                monthlyBalanceRDToInsert.AddedBy = User.Identity.Name;

                monthlyAccBalancesToInsert.Add(monthlyBalanceRDToInsert);

                //Canteen

                MonthlyBalance monthlyBalanceCanteenToInsert = new MonthlyBalance();
                monthlyBalanceCanteenToInsert.Id = Guid.NewGuid();
                monthlyBalanceCanteenToInsert.Account = accountDataService.GetAccountsByName(ExcelReader.GetCellValue(ms, "Sheet1", "A4"));
                monthlyBalanceCanteenToInsert.AddedTime = DateTime.Now;
                monthlyBalanceCanteenToInsert.Balance = ExcelReader.GetCellValue(ms, "Sheet1", "B4");
                monthlyBalanceCanteenToInsert.IsLatest = true;
                monthlyBalanceCanteenToInsert.Month = month + " " + year;
                monthlyBalanceCanteenToInsert.Order = orderNo;
                monthlyBalanceCanteenToInsert.AddedBy = User.Identity.Name;

                monthlyAccBalancesToInsert.Add(monthlyBalanceCanteenToInsert);

                //CEO’s car

                MonthlyBalance monthlyBalanceCarToInsert = new MonthlyBalance();
                monthlyBalanceCarToInsert.Id = Guid.NewGuid();
                monthlyBalanceCarToInsert.Account = accountDataService.GetAccountsByName(ExcelReader.GetCellValue(ms, "Sheet1", "A5"));
                monthlyBalanceCarToInsert.AddedTime = DateTime.Now;
                monthlyBalanceCarToInsert.Balance = ExcelReader.GetCellValue(ms, "Sheet1", "B5");
                monthlyBalanceCarToInsert.IsLatest = true;
                monthlyBalanceCarToInsert.Month = month + " " + year;
                monthlyBalanceCarToInsert.Order = orderNo;
                monthlyBalanceCarToInsert.AddedBy = User.Identity.Name;

                monthlyAccBalancesToInsert.Add(monthlyBalanceCarToInsert);

                //Marketing

                MonthlyBalance monthlyBalanceMarketingToInsert = new MonthlyBalance();
                monthlyBalanceMarketingToInsert.Id = Guid.NewGuid();
                monthlyBalanceMarketingToInsert.Account = accountDataService.GetAccountsByName(ExcelReader.GetCellValue(ms, "Sheet1", "A6"));
                monthlyBalanceMarketingToInsert.AddedTime = DateTime.Now;
                monthlyBalanceMarketingToInsert.Balance = ExcelReader.GetCellValue(ms, "Sheet1", "B6");
                monthlyBalanceMarketingToInsert.IsLatest = true;
                monthlyBalanceMarketingToInsert.Month = month + " " + year;
                monthlyBalanceMarketingToInsert.Order = orderNo;
                monthlyBalanceMarketingToInsert.AddedBy = User.Identity.Name;

                monthlyAccBalancesToInsert.Add(monthlyBalanceMarketingToInsert);

                //Parking fines

                MonthlyBalance monthlyBalanceParkingToInsert = new MonthlyBalance();
                monthlyBalanceParkingToInsert.Id = Guid.NewGuid();
                monthlyBalanceParkingToInsert.Account = accountDataService.GetAccountsByName(ExcelReader.GetCellValue(ms, "Sheet1", "A7"));
                monthlyBalanceParkingToInsert.AddedTime = DateTime.Now;
                monthlyBalanceParkingToInsert.Balance = ExcelReader.GetCellValue(ms, "Sheet1", "B7");
                monthlyBalanceParkingToInsert.IsLatest = true;
                monthlyBalanceParkingToInsert.Month = month + " " + year;
                monthlyBalanceParkingToInsert.Order = orderNo;
                monthlyBalanceParkingToInsert.AddedBy = User.Identity.Name;

                monthlyAccBalancesToInsert.Add(monthlyBalanceParkingToInsert);


                accountDataService.AddAccounts(monthlyAccBalancesToInsert);
            }

            //CSV File Read
            catch (Exception e)
            {
                try
                {
                    TextReader textReader = new StreamReader(ms);
                    string heading = textReader.ReadLine();

                    string[] headingChunks = heading.Split(' ');
                    string year = headingChunks.Last();
                    int lastTwoIndex = headingChunks.Count() - 2;
                    string month = headingChunks[lastTwoIndex];
                    string iMonthNo = Convert.ToDateTime("01-" + month + "-" + year).Month.ToString("D2");
                    string idString = year + iMonthNo;
                    int orderNo = Int32.Parse(idString);

                    var csv = new CsvReader(textReader);

                    csv.Configuration.HasHeaderRecord = false;

                    var balances = csv.GetRecords<MonthlyAccountBalanceViewModel>().ToList();

                    List<MonthlyBalance> monthlyAccBalancesToInsert = new List<MonthlyBalance>();

                    foreach (MonthlyAccountBalanceViewModel monthlyBalancesAcc in balances)
                    {
                        MonthlyBalance monthlyBalanceRecToInsert = new MonthlyBalance();
                        monthlyBalanceRecToInsert.Id = Guid.NewGuid();
                        monthlyBalanceRecToInsert.Account = accountDataService.GetAccountsByName(monthlyBalancesAcc.DisplayAccountName);
                        monthlyBalanceRecToInsert.AddedTime = DateTime.Now;
                        monthlyBalanceRecToInsert.Balance = monthlyBalancesAcc.DisplayAccountBalance;
                        monthlyBalanceRecToInsert.IsLatest = true;
                        monthlyBalanceRecToInsert.Month = month + " " + year;
                        monthlyBalanceRecToInsert.Order = orderNo;
                        monthlyBalanceRecToInsert.AddedBy = User.Identity.Name;

                        monthlyAccBalancesToInsert.Add(monthlyBalanceRecToInsert);
                    }

                    accountDataService.AddAccounts(monthlyAccBalancesToInsert);
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        
        // PUT: api/MonthlyBalance/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
