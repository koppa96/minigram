using System;
using System.Threading.Tasks;
using Minigram.Application.Features.FriendManagement.Interface.Dtos;

namespace Minigram.Application.Features.FriendManagement.Interface.Clients
{
    public interface IFriendshipClient
    {
        Task FriendshipCreated(FriendshipDto dto);
        Task FriendshipDeleted(Guid friendshipId);
    }
}