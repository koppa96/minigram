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
        
        public TSubClient User(Guid userId)
        {
            return hubContext.Clients.User(userId.ToString());
        }

        public TSubClient Users(IEnumerable<Guid> userIds)
        {
            return hubContext.Clients.Users(userIds.Select(x => x.ToString()));
        }

        public TSubClient Users(params Guid[] userIds)
        {
            // Avoid recursion, call .AsEnumerable()
            return Users(userIds.AsEnumerable());
        }
    }
}