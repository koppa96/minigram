using System;
using System.Threading.Tasks;
using Minigram.Application.Features.Conversations.Interface.Dtos;

namespace Minigram.Application.Features.Conversations.Interface.Clients
{
    public interface IMessageClient
    {
        Task MessageReceived(Guid conversationId, MessageDto dto);
    }
}