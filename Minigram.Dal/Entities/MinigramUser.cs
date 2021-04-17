using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Minigram.Dal.Abstractions;

namespace Minigram.Dal.Entities
{
    public class MinigramUser : IdentityUser<Guid>, ISoftDelete
    {
        public bool IsDeleted { get; set; }
        
        // EF Core doesn't support using the same navigation collection for two navigation properties
        // so we need separate nav props for friendships where we are the first friend, and where we are the second friend
        public virtual ICollection<Friendship> Friendships1 { get; set; }
        public virtual ICollection<Friendship> Friendships2 { get; set; }
        
        // Provide nicer interface
        public IEnumerable<Friendship> Friendships => Friendships1.Concat(Friendships2);
        public IEnumerable<MinigramUser> Friends => Friendships.Select(x => x.User1Id == Id ? x.User2 : x.User1);

        public virtual ICollection<FriendRequest> SentRequests { get; set; }
        public virtual ICollection<FriendRequest> ReceivedRequests { get; set; }
        
        public virtual ICollection<ConversationMembership> ConversationMemberships { get; set; }
    }
}