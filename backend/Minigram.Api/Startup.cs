using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Minigram.Api.Extensions;
using Minigram.Api.Services;
using Minigram.Api.SignalR;
using Minigram.Application.Abstractions.Services;
using Minigram.Application.Abstractions.Services.Notification;
using Minigram.Application.Features.FriendManagement.Interface.Clients;
using static Minigram.Api.Resources.AuthorizationConstants;
using Minigram.Dal;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace Minigram.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MinigramDbContext>(options =>
                options.EnableDetailedErrors(Environment.IsDevelopment())
                    .EnableSensitiveDataLogging(Environment.IsDevelopment())
                    .UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddControllers();
            services.AddSignalR();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration.GetValue<string>("Authentication:Authority");
                    options.Audience = Configuration.GetValue<string>("Authentication:Audience");
                    options.RequireHttpsMetadata = false;
                });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy => policy.WithOrigins(Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>())
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });

            services.AddAuthorization(options =>
            {
                // There is no role based authorization in the app, as all users are in the same role
                // But there is a scope based authorization for the clients.
                // The client app can only execute the request if it has the required scope
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

            services.AddNotificationService<NotificationHub, INotificationClient>();

            services.AddHttpContextAccessor();
            services.AddScoped<IIdentityService, IdentityService>();
            
            services.AddOpenApiDocument(config =>
            {
                config.Title = "Minigram API";
                config.Description = "A minimalistic message server API that supports group chats.";
                config.DocumentName = "Minigram";

                config.AddSecurity("OAuth2", new OpenApiSecurityScheme
                {
                    OpenIdConnectUrl =
                        $"{Configuration.GetValue<string>("Authentication:Authority")}/.well-known/openid-configuration",
                    Scheme = "Bearer",
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl =
                                $"{Configuration.GetValue<string>("Authentication:Authority")}/connect/authorize",
                            TokenUrl = $"{Configuration.GetValue<string>("Authentication:Authority")}/connect/token",
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "OpenId" },
                                { Scopes.Conversations.Read, "Read conversations" },
                                { Scopes.Conversations.Manage, "Manage conversations" },
                                { Scopes.Friendships.Read, "Read friendships and friend requests" },
                                { Scopes.Friendships.Manage, "Manage friendships and friend requests"}
                            }
                        }
                    }
                });

                config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("OAuth2"));
            });

            services.AddAutoMapper(Assembly.Load("Minigram.Application"));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Minigram.Application"))
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
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
            app.UseCors();
            
            app.UseOpenApi();
            app.UseSwaggerUi3(config =>
            {
                config.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = "minigram-swagger",
                    ClientSecret = null,
                    UsePkceWithAuthorizationCodeGrant = true,
                    ScopeSeparator = " ",
                    Realm = null,
                    AppName = "Minigram Swagger Client"
                };
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notifications");
            });
        }
    }
}
