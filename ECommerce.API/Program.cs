using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Core.Entities.Identity;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ECommerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<ECommerceContext>();

                    await context.Database.MigrateAsync();

                    await ECommerceContextSeed.SeedAsync(context, loggerFactory);

                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();

                    var userManager = services.GetRequiredService<UserManager<AppUser>>();

                    await identityContext.Database.MigrateAsync();

                    await AppIdentityDbContextSeed.SeedUserAsync(userManager);
                }
                catch (System.Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occured during migration");
                }

                host.Run();
            }
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
