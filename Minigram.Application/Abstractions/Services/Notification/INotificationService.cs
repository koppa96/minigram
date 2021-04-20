using System;
using System.Collections.Generic;

namespace Minigram.Application.Abstractions.Services.Notification
{
    public interface INotificationService<out TClient>
    {
        TClient User(Guid userId);
        TClient Users(IEnumerable<Guid> userIds);
        TClient Users(params Guid[] userIds);
    }
}