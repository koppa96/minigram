using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Minigram.Application.Abstractions.Services;
using Minigram.Application.Abstractions.Services.Notification;
using Minigram.Application.Extensions;
using Minigram.Application.Features.Conversations.Interface.Clients;
using Minigram.Application.Features.Conversations.Interface.Dtos;
using Minigram.Application.Features.Conversations.Interface.Services;
using Minigram.Dal;
using Minigram.Dal.Entities;
using Minigram.Dal.Extensions;

namespace Minigram.Application.Features.Conversations.Services
{
    public class ConversationMemberService : IConversationMemberService
    {
        private readonly MinigramDbContext context;
        private readonly IIdentityService identityService;
        private readonly IMapper mapper;
        private readonly INotificationService<IConversationClient> notificationService;

        public ConversationMemberService(
            MinigramDbContext context,
            IIdentityService identityService,
            IMapper mapper,
            INotificationService<IConversationClient> notificationService)
        {
            this.context = context;
            this.identityService = identityService;
            this.mapper = mapper;
            this.notificationService = notificationService;
        }
        
        public async Task<ConversationMembershipDto> AddMembershipAsync(Guid conversationId, Guid userId, CancellationToken cancellationToken = default)
        {
            var conversation = await context.Conversations.Include(x => x.ConversationMemberships)
                    .ThenInclude(x => x.Member)
                .FindByIdAsync(conversationId, cancellationToken);

            if (!conversation.IsAdmin(identityService.CurrentUserId))
            {
                throw new UnauthorizedAccessException("Only an administrator can add a user to a conversation.");
            }

            var newMember = await context.Users.FindByIdAsync(userId, cancellationToken);
            var membership = new ConversationMembership
            {
                Conversation = conversation,
                Member = newMember
            };
            context.ConversationMemberships.Add(membership);
            await context.SaveChangesAsync(cancellationToken);

            await notificationService.Users(conversation.ConversationMemberships
                    .Where(x => x.MemberId != identityService.CurrentUserId && x.MemberId != userId)
                    .Select(x => x.MemberId))
                .ConversationUpdated(mapper.Map<ConversationDetailsDto>(conversation));

            await notificationService.User(userId)
                .AddedToConversation(mapper.Map<ConversationListDto>(conversation));

            return mapper.Map<ConversationMembershipDto>(membership);
        }

        public async Task<ConversationMembershipDto> EditMembershipAsync(Guid membershipId, ConversationMembershipEditDto dto,
            CancellationToken cancellationToken = default)
        {
            var membership = await context.ConversationMemberships.Include(x => x.Conversation)
                    .ThenInclude(x => x.ConversationMemberships)
                        .ThenInclude(x => x.Member)
                .FindByIdAsync(membershipId, cancellationToken);

            var currentUserMembership = membership.Conversation.ConversationMemberships.Single(
                x => x.MemberId == identityService.CurrentUserId);

            if (!currentUserMembership.IsAdmin)
            {
                throw new UnauthorizedAccessException("Only an administrator can modify the permissions of a user.");
            }

            membership.IsAdmin = dto.IsAdmin;
            await context.SaveChangesAsync(cancellationToken);

            await notificationService.Users(membership.Conversation.ConversationMemberships
                    .Where(x => x.MemberId != identityService.CurrentUserId)
                    .Select(x => x.MemberId))
                .ConversationUpdated(mapper.Map<ConversationDetailsDto>(membership.Conversation));

            return mapper.Map<ConversationMembershipDto>(membership);
        }

        public async Task RemoveMembershipAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var membership = await context.ConversationMemberships.Include(x => x.Conversation)
                    .ThenInclude(x => x.ConversationMemberships)
                        .ThenInclude(x => x.Member)
                .FindByIdAsync(id, cancellationToken);

            if (membership.MemberId != identityService.CurrentUserId &&
                !membership.Conversation.ConversationMemberships.Any(
                    x => x.IsAdmin && x.MemberId == identityService.CurrentUserId))
            {
                throw new UnauthorizedAccessException("Only an administrator or the user itself can remove a user from a conversation.");
            }

            context.ConversationMemberships.Remove(membership);
            await context.SaveChangesAsync(cancellationToken);

            await notificationService.User(membership.MemberId)
                .RemovedFromConversation(id);

            await notificationService.Users(membership.Conversation.ConversationMemberships.Where(x => x != membership)
                    .Select(x => x.MemberId))
                .ConversationUpdated(mapper.Map<ConversationDetailsDto>(membership.Conversation));
        }
    }
}