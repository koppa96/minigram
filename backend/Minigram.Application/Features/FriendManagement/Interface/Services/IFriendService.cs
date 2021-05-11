using System;
using System.Threading;
using System.Threading.Tasks;
using Minigram.Application.Abstractions.Dtos;
using Minigram.Application.Features.FriendManagement.Interface.Dtos;

namespace Minigram.Application.Features.FriendManagement.Interface.Services
{
    public interface IFriendService
    {
        Task<PagedListDto<FriendshipDto>> ListFriendshipsAsync(string searchText, int pageIndex, int pageSize,
            CancellationToken cancellationToken = default);

        Task<FriendshipDto> CreateFriendshipAsync(Guid requestId, CancellationToken cancellationToken = default);

        Task DeleteFriendshipAsync(Guid friendshipId, CancellationToken cancellationToken = default);
    }
}