using System;
using System.Collections.Generic;
using Minigram.Dal.Abstractions;

namespace Minigram.Dal.Entities
{
    public class ConversationMembership : IEntity
    {
        public Guid Id { get; set; }

        public Guid MemberId { get; set; }
        public virtual MinigramUser Member { get; set; }

        public Guid ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }

        public bool IsAdmin { get; set; }

        public virtual ICollection<Message> SentMessages { get; set; }
    }
}