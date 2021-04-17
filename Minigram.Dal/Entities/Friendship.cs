using System;
using Minigram.Dal.Abstractions;

namespace Minigram.Dal.Entities
{
    public class Friendship : IEntity
    {
        public Guid Id { get; set; }

        public Guid User1Id { get; set; }
        public virtual MinigramUser User1 { get; set; }

        public Guid User2Id { get; set; }
        public virtual MinigramUser User2 { get; set; }
    }
}