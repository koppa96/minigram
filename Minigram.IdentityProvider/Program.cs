using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Minigram.Dal;
using Minigram.IdentityProvider.Extensions;

namespace Minigram.IdentityProvider
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var minigramDbContext = scope.ServiceProvider.GetRequiredService<MinigramDbContext>();
                await minigramDbContext.Database.MigrateAsync();

                var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                await persistedGrantDbContext.Database.MigrateAsync();

                var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                await configurationDbContext.Database.MigrateAsync();

                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                await configurationDbContext.InitializeFromConfiguration(configuration);
            }
            
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
