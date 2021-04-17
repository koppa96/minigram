using System;
using Minigram.Application.Features.Users.Dtos;

namespace Minigram.Application.Features.FriendManagement.Interface.Dtos
{
    public class FriendRequestDto
    {
        public Guid Id { get; set; }
        public UserListDto Sender { get; set; }
        public UserListDto Recipient { get; set; }
    }
}