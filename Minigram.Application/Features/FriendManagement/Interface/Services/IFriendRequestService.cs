using System;
using System.Threading;
using System.Threading.Tasks;
using Minigram.Application.Abstractions.Dtos;
using Minigram.Application.Features.FriendManagement.Interface.Dtos;

namespace Minigram.Application.Features.FriendManagement.Interface.Services
{
    public interface IFriendRequestService
    {
        Task<PagedListDto<FriendRequestDto>> ListSentRequestsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = default);

        Task<PagedListDto<FriendRequestDto>> ListReceivedRequestsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = default);
        
        Task<FriendRequestDto> SendRequestAsync(Guid recipientId, CancellationToken cancellationToken = default);

        Task DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    }
}