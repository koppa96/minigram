using System;
using System.Collections.Generic;

namespace Minigram.Application.Features.Conversations.Interface.Dtos
{
    public class ConversationDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<ConversationMembershipDto> Memberships { get; set; }
    }
}