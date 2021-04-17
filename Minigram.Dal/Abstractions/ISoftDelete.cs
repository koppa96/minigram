namespace Minigram.Dal.Abstractions
{
    public interface ISoftDelete : IEntity
    {
        public bool IsDeleted { get; set; }
    }
}