using Microsoft.AspNetCore.SignalR;

namespace Minigram.Api.SignalR
{
    public class NotificationHub : Hub<INotificationClient>
    {
    }
}