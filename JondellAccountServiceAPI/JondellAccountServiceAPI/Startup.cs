using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Jondell.AccountService;
using Jondell.AccountService.Data;
using Microsoft.EntityFrameworkCore;
using JondellAccountServiceAPI.Extensions;
using JondellAccountServiceAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace JondellAccountServiceAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //Setting up the db context
            services.ConfigureDbContext(Configuration);

            //Injecting the appropriate data service
            services.ConfigureDataService();

            //Configure authentication service
            services.ConfigureAuthentication();

            services.ConfigureCors();

            services.AddIdentity<IdentityUser, IdentityRole>()
       .AddEntityFrameworkStores<AccountServiceDbContext>()
       .AddDefaultTokenProviders();
            //services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();



        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles   
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database  
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            IdentityUser adminUser = await UserManager.FindByEmailAsync("sundt@jondell.com");

            if (adminUser == null)
            {
                adminUser = new IdentityUser()
                {
                    UserName = "sundt@jondell.com",
                    Email = "sundt@jondell.com",
                };
                await UserManager.CreateAsync(adminUser, "PassworD1.");
            }
            await UserManager.AddToRoleAsync(adminUser, "Admin");


            IdentityUser normalUser = await UserManager.FindByEmailAsync("john@jondell.com");

            if (normalUser == null)
            {
                normalUser = new IdentityUser()
                {
                    UserName = "john@jondell.com",
                    Email = "john@jondell.com",
                };
                await UserManager.CreateAsync(normalUser, "PassworD2.");
            }
            await UserManager.AddToRoleAsync(normalUser, "User");

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.UseMvc();

            CreateRoles(serviceProvider).Wait();

        }
    }
}
