using System;

namespace Minigram.Application.Features.Conversations.Interface.Dtos
{
    public class ConversationListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public MessageDto LastMessage { get; set; }
    }
}