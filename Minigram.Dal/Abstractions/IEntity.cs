using System;

namespace Minigram.Dal.Abstractions
{
    public interface IEntity
    {
        public Guid Id { get; set; }
    }
}