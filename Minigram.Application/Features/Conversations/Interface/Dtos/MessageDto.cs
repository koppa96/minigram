using System;
using Minigram.Application.Features.Users.Interface.Dtos;

namespace Minigram.Application.Features.Conversations.Interface.Dtos
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public UserListDto Sender { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}