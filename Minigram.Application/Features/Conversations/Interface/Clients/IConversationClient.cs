using System;
using System.Threading.Tasks;
using Minigram.Application.Features.Conversations.Interface.Dtos;

namespace Minigram.Application.Features.Conversations.Interface.Clients
{
    public interface IConversationClient
    {
        Task AddedToConversation(ConversationListDto dto);
        Task RemovedFromConversation(Guid conversationId);
        Task ConversationUpdated(ConversationDetailsDto dto);
    }
}