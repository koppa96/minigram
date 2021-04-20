using System;
using System.Threading;
using System.Threading.Tasks;
using Minigram.Application.Features.Conversations.Interface.Dtos;

namespace Minigram.Application.Features.Conversations.Interface.Services
{
    public interface IConversationService
    {
        Task<ConversationDetailsDto> CreateConversationAsync(ConversationCreateEditDto dto,
            CancellationToken cancellationToken = default);

        Task<ConversationDetailsDto> UpdateConversationAsync(Guid id, ConversationCreateEditDto dto,
            CancellationToken cancellationToken = default);

        Task DeleteConversationAsync(Guid id, CancellationToken cancellationToken = default);
    }
}