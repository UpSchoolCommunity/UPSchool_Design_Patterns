using Command_Design_Pattern.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Command_Design_Pattern
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            identityDbContext.Database.Migrate();

            if (!userManager.Users.Any())
            {
                userManager.CreateAsync(new AppUser()
                {
                    UserName = "User1",
                    Email = "user1@outlook.com"

                }, "123").Wait();

                userManager.CreateAsync(new AppUser()
                {
                    UserName = "User2",
                    Email = "user2@outlook.com"

                }, "123").Wait();

                userManager.CreateAsync(new AppUser()
                {
                    UserName = "User3",
                    Email = "user3@outlook.com"

                }, "123").Wait();

                userManager.CreateAsync(new AppUser()
                {
                    UserName = "User4",
                    Email = "user4@outlook.com"

                }, "123").Wait();

                userManager.CreateAsync(new AppUser()
                {
                    UserName = "User5",
                    Email = "user5@outlook.com"

                }, "123").Wait();
            }


            Enumerable.Range(1, 30).ToList().ForEach(x =>
            {
                identityDbContext.Add(new Product() { Name = $"kalem {x}", Price = x * 100, Stock = x + 50 });
            });

            identityDbContext.SaveChanges();

            host.Run();
        }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
