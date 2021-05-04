using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Minigram.Api.Services;
using Minigram.Application.Abstractions.Services.Notification;

namespace Minigram.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotificationService<THub, TClient>(this IServiceCollection services)
            where THub : Hub<TClient>
            where TClient : class
        {
            foreach (var @interface in typeof(TClient).GetInterfaces())
            {
                services.AddTransient(
                    typeof(INotificationService<>).MakeGenericType(@interface),
                    typeof(NotificationService<,,>).MakeGenericType(typeof(THub), typeof(TClient), @interface));
            }

            return services;
        }
    }
}