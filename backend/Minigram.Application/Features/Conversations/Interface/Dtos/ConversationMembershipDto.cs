using System;
using Minigram.Application.Features.Users.Interface.Dtos;

namespace Minigram.Application.Features.Conversations.Interface.Dtos
{
    public class ConversationMembershipDto
    {
        public Guid Id { get; set; }
        public UserListDto Member { get; set; }
        public bool IsAdmin { get; set; }
    }
}