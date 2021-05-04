using Minigram.Application.Features.Conversations.Interface.Clients;
using Minigram.Application.Features.FriendManagement.Interface.Clients;

namespace Minigram.Api.SignalR
{
    public interface INotificationClient : IFriendManagementClient, IConversationClient, IMessageClient
    {
    }
}