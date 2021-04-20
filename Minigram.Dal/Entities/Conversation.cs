using System;
using System.Collections.Generic;
using Minigram.Dal.Abstractions;

namespace Minigram.Dal.Entities
{
    public class Conversation : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<ConversationMembership> ConversationMemberships { get; set; }
    }
}