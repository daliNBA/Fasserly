using Fasserly.Database;
using Fasserly.Database.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Fasserly.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                try
                {
                    var context = service.GetRequiredService<DatabaseContext>();
                    var userManager = service.GetRequiredService<UserManager<UserFasserly>>();
                    context.Database.Migrate();
                    Seed.SeedData(context, userManager).Wait();
                }
                catch (Exception ex)
                {
                    var logger = service.GetRequiredService<ILogger>();
                    logger.LogError(ex, "error occured while migration");
                }
            }
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
