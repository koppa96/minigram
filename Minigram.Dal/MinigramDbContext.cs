using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Minigram.Dal.Entities;

namespace Minigram.Dal
{
    public class MinigramDbContext : IdentityDbContext<MinigramUser, IdentityRole<Guid>, Guid>
    {
        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<FriendRequest> FriendRequests { get; set; }
        public virtual DbSet<ConversationMembership> ConversationMemberships { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        public MinigramDbContext(DbContextOptions<MinigramDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Friendship>(friendship =>
            {
                friendship.HasOne(x => x.User1)
                    .WithMany(x => x.Friendships1)
                    .HasForeignKey(x => x.User1Id);

                friendship.HasOne(x => x.User2)
                    .WithMany(x => x.Friendships2)
                    .HasForeignKey(x => x.User2);

                friendship.HasIndex(x => new
                {
                    x.User1Id,
                    x.User2Id
                }).IsUnique();
            });

            builder.Entity<ConversationMembership>(conversationMembership =>
            {
                conversationMembership.HasOne(x => x.Member)
                    .WithMany(x => x.ConversationMemberships)
                    .HasForeignKey(x => x.MemberId);

                conversationMembership.HasOne(x => x.Conversation)
                    .WithMany(x => x.ConversationMemberships)
                    .HasForeignKey(x => x.ConversationId);

                conversationMembership.HasIndex(x => new
                {
                    x.ConversationId,
                    x.MemberId
                }).IsUnique();
            });

            builder.Entity<Message>(message =>
            {
                message.HasOne(x => x.ConversationMembership)
                    .WithMany()
                    .HasForeignKey(x => x.ConversationMembershipId);
            });

            builder.Entity<FriendRequest>(friendRequest =>
            {
                friendRequest.HasOne(x => x.Sender)
                    .WithMany(x => x.SentRequests)
                    .HasForeignKey(x => x.SenderId);

                friendRequest.HasOne(x => x.Recipient)
                    .WithMany(x => x.ReceivedRequests)
                    .HasForeignKey(x => x.RecipientId);

                friendRequest.HasIndex(x => new
                {
                    x.RecipientId,
                    x.SenderId
                }).IsUnique();
            });
        }
    }
}