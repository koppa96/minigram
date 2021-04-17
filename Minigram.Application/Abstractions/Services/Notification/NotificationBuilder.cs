using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minigram.Application.Abstractions.Services.Notification
{
    public class NotificationBuilder<TClient>
    {
        private readonly INotificationService<TClient> service;
        public IEnumerable<Guid> UserIds { get; }
        public List<Func<TClient, Task>> Callbacks { get; } = new();

        public NotificationBuilder(IEnumerable<Guid> userIds, INotificationService<TClient> service)
        {
            this.service = service;
            UserIds = userIds;
        }

        public NotificationBuilder<TClient> That(Func<TClient, Task> callback)
        {
            Callbacks.Add(callback);
            return this;
        }

        public Task SendAsync()
        {
            return service.SendAsync(this);
        }
    }
}