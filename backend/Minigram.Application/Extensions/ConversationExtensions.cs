using System;
using System.Linq;
using Minigram.Dal.Entities;

namespace Minigram.Application.Extensions
{
    public static class ConversationExtensions
    {
        public static bool IsAdmin(this Conversation conversation, Guid userId)
        {
            return conversation.ConversationMemberships.Any(x => x.IsAdmin && x.MemberId == userId);
        }
    }
}