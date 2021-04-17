using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Minigram.Api.Extensions;
using Minigram.Application.Abstractions.Services.Notification;

namespace Minigram.Api.Services
{
    public class NotificationService<THub, TClient, TSubClient> : INotificationService<TSubClient>
        where TClient : class, TSubClient
        where THub : Hub<TClient>
    {
        private readonly IHubContext<THub, TClient> hubContext;

        public NotificationService(IHubContext<THub, TClient> hubContext)
        {
            this.hubContext = hubContext;
        }
        
        public NotificationBuilder<TSubClient> NotifyUser(Guid userId)
        {
            return new(EnumerableExtensions.From(userId), this);
        }

        public NotificationBuilder<TSubClient> NotifyUsers(IEnumerable<Guid> userIds)
        {
            return new(userIds, this);
        }

        public NotificationBuilder<TSubClient> NotifyUsers(params Guid[] userIds)
        {
            return new(userIds, this);
        }

        public async Task SendAsync(NotificationBuilder<TSubClient> notificationBuilder)
        {
            var userRefs = hubContext.Clients.Users(notificationBuilder.UserIds.Select(x => x.ToString()));
            foreach (var callback in notificationBuilder.Callbacks)
            {
                await callback(userRefs);
            }
        }
    }
}