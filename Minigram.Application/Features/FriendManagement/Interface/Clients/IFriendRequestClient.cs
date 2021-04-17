using System;
using System.Threading.Tasks;
using Minigram.Application.Features.FriendManagement.Interface.Dtos;

namespace Minigram.Application.Features.FriendManagement.Interface.Clients
{
    public interface IFriendRequestClient
    {
        Task FriendRequestCreated(FriendRequestDto dto);
        Task FriendRequestDeleted(Guid requestId);
    }
}