using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Minigram.Api.SignalR
{
    [Authorize]
    public class NotificationHub : Hub<INotificationClient>
    {
    }
}