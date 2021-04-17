using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minigram.Application.Abstractions.Services.Notification
{
    public interface INotificationService<TClient>
    {
        NotificationBuilder<TClient> NotifyUser(Guid userId);
        NotificationBuilder<TClient> NotifyUsers(IEnumerable<Guid> userIds);
        NotificationBuilder<TClient> NotifyUsers(params Guid[] userIds);
        Task SendAsync(NotificationBuilder<TClient> notificationBuilder);
    }
}