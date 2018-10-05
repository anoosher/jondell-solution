using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JondellAccountServiceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JondellAccountServiceAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

        // GET api/values
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName,user.Password,false, false);


            if (result.Succeeded)
            {
                String issuer = "";
                String audience = "";
                
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                {
                    issuer = "https://jondellaccountserviceapi.azurewebsites.net";
                    audience = "http://localhost:4200";
                }
                else if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    issuer = "http://localhost:57684";
                    audience = "http://localhost:4200";
                }


                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JondellsSuperKey"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var userLoggedIn = await _userManager.FindByEmailAsync(user.UserName);
                var roles = await _userManager.GetRolesAsync(userLoggedIn);

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, userLoggedIn.Email),
                    new Claim(ClaimTypes.Role, roles.First().ToString())
                };




                var tokeOptions = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString , DisplayName = userLoggedIn.UserName, Role= roles.First().ToString() });
            }
            else
            {
                return Unauthorized();
            }
        }

       
    }
}