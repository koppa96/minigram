using System;
using Minigram.Dal.Abstractions;

namespace Minigram.Dal.Entities
{
    public class FriendRequest : IEntity
    {
        public Guid Id { get; set; }

        public Guid SenderId { get; set; }
        public virtual MinigramUser Sender { get; set; }

        public Guid RecipientId { get; set; }
        public virtual MinigramUser Recipient { get; set; }
    }
}