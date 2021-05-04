using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Minigram.IdentityProvider.Extensions
{
    public static class ConfigurationDbContextExtensions
    {
        public static async Task InitializeFromConfiguration(this ConfigurationDbContext context,
            IConfiguration configuration)
        {
            await context.AddIdentityResourcesIfNoneExist(configuration);
            await context.AddApiScopesIfNoneExist(configuration);
            await context.AddApiResourcesIfNoneExist(configuration);
            await context.AddClientsIfNoneExist(configuration);

            await context.SaveChangesAsync();
        }

        private static async Task AddIdentityResourcesIfNoneExist(this ConfigurationDbContext context, IConfiguration configuration)
        {
            if (!await context.IdentityResources.AnyAsync())
            {
                var identityResources = new List<IdentityResource>();
                configuration.GetSection("IdentityServer:IdentityResources").Bind(identityResources);
            
                context.IdentityResources.AddRange(identityResources.Select(x => x.ToEntity()));
            }
        }
        
        private static async Task AddApiScopesIfNoneExist(this ConfigurationDbContext context, IConfiguration configuration)
        {
            if (!await context.ApiScopes.AnyAsync())
            {
                var apiScopes = new List<ApiScope>();
                configuration.GetSection("IdentityServer:ApiScopes").Bind(apiScopes);
            
                context.ApiScopes.AddRange(apiScopes.Select(x => x.ToEntity()));
            }
        }
        
        private static async Task AddApiResourcesIfNoneExist(this ConfigurationDbContext context, IConfiguration configuration)
        {
            if (!await context.ApiResources.AnyAsync())
            {
                var apiResources = new List<ApiResource>();
                configuration.GetSection("IdentityServer:ApiResources").Bind(apiResources);
            
                context.ApiResources.AddRange(apiResources.Select(x => x.ToEntity()));
            }
        }
        
        private static async Task AddClientsIfNoneExist(this ConfigurationDbContext context, IConfiguration configuration)
        {
            if (!await context.Clients.AnyAsync())
            {
                var clients = new List<Client>();
                configuration.GetSection("IdentityServer:Clients").Bind(clients);
            
                context.Clients.AddRange(clients.Select(x => x.ToEntity()));
            }
        }
    }
}