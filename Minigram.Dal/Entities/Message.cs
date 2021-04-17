using System;
using Minigram.Dal.Abstractions;

namespace Minigram.Dal.Entities
{
    public class Message : IEntity
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public Guid ConversationMembershipId { get; set; }
        public virtual ConversationMembership ConversationMembership { get; set; }
    }
}