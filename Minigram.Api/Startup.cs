using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using static Minigram.Api.Resources.AuthorizationConstants;
using Minigram.Dal;

namespace Minigram.Api
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
            services.AddDbContext<MinigramDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddControllers();

            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration.GetValue<string>("Authentication:Authority");
                    options.Audience = Configuration.GetValue<string>("Authentication:Audience");
                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthorization(options =>
            {
                // There is no role based authorization in the app, as all users are in the same role
                // But there is a scope based authorization.
                // The client app can only execute the request 
                options.AddPolicy(Scopes.Friendships.Read, policy => policy.RequireAuthenticatedUser()
                    .RequireClaim(ScopeClaimType, Scopes.Friendships.Read)
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                
                options.AddPolicy(Scopes.Friendships.Manage, policy => policy.RequireAuthenticatedUser()
                    .RequireClaim(ScopeClaimType, Scopes.Friendships.Manage)
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                
                options.AddPolicy(Scopes.Conversations.Read, policy => policy.RequireAuthenticatedUser()
                    .RequireClaim(ScopeClaimType, Scopes.Conversations.Read)
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                
                options.AddPolicy(Scopes.Conversations.Manage, policy => policy.RequireAuthenticatedUser()
                    .RequireClaim(ScopeClaimType, Scopes.Conversations.Manage)
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));

                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
