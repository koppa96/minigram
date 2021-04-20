using System;
using System.Threading;
using System.Threading.Tasks;
using Minigram.Application.Features.Conversations.Interface.Dtos;

namespace Minigram.Application.Features.Conversations.Interface.Services
{
    public interface IConversationMemberService
    {
        Task<ConversationMembershipDto> AddMembershipAsync(Guid conversationId, Guid userId, CancellationToken cancellationToken = default);

        Task<ConversationMembershipDto> EditMembershipAsync(Guid membershipId, ConversationMembershipEditDto dto,
            CancellationToken cancellationToken = default);
        
        Task RemoveMembershipAsync(Guid id, CancellationToken cancellationToken = default);
    }
}