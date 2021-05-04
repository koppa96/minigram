using System;

namespace Minigram.Application.Features.FriendManagement.Interface.Dtos
{
    public class FriendRequestCreateDto
    {
        public Guid RecipientId { get; set; }
    }
}