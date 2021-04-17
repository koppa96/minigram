using System;
using Minigram.Application.Features.Users.Dtos;

namespace Minigram.Application.Features.FriendManagement.Interface.Dtos
{
    public class FriendshipDto
    {
        public Guid Id { get; set; }
        public UserListDto Friend { get; set; }
    }
}