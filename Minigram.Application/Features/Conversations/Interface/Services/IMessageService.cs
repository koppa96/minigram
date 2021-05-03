using System;
using System.Threading;
using System.Threading.Tasks;
using Minigram.Application.Abstractions.Dtos;
using Minigram.Application.Features.Conversations.Interface.Dtos;

namespace Minigram.Application.Features.Conversations.Interface.Services
{
    public interface IMessageService
    {
        Task<PagedListDto<MessageDto>> ListMessagesAsync(Guid conversationId, int pageIndex, int pageSize,
            CancellationToken cancellationToken = default);
        Task<MessageDto> SendAsync(Guid conversationId, string text, CancellationToken cancellationToken = default);
    }
}