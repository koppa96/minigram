using System;
using System.Threading;
using System.Threading.Tasks;
using Minigram.Application.Abstractions.Dtos;
using Minigram.Application.Features.Conversations.Interface.Dtos;

namespace Minigram.Application.Features.Conversations.Interface.Services
{
    public interface IConversationService
    {
        Task<PagedListDto<ConversationListDto>> ListConversationsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = default);

        Task<ConversationDetailsDto> GetConversationAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<ConversationDetailsDto> CreateConversationAsync(ConversationCreateEditDto dto,
            CancellationToken cancellationToken = default);

        Task<ConversationDetailsDto> UpdateConversationAsync(Guid id, ConversationCreateEditDto dto,
            CancellationToken cancellationToken = default);

        Task DeleteConversationAsync(Guid id, CancellationToken cancellationToken = default);
    }
}