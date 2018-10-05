using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jondell.AccountService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JondellAccountServiceAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {

        private readonly IAccountDataService accountDataService;

        public AccountController(IAccountDataService accountDataService)
        {
            this.accountDataService = accountDataService;
        }

        // GET: api/Account
        [HttpGet, Authorize]
        public IEnumerable<Account> Get()
        {
            return accountDataService.GetAccounts();
        }

        // GET: api/Account/5
        [HttpGet("{id}", Name = "GetAccount")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Account
        [HttpPost]
        public void Post([FromBody]Account account)
        {
            accountDataService.AddAccount(account);
        }
        
        // PUT: api/Account/5
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
