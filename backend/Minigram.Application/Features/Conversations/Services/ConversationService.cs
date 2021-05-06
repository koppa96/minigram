using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Minigram.Application.Abstractions.Dtos;
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
    public class ConversationService : IConversationService
    {
        private readonly MinigramDbContext context;
        private readonly IIdentityService identityService;
        private readonly IMapper mapper;
        private readonly INotificationService<IConversationClient> notificationService;

        public ConversationService(
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

        public Task<PagedListDto<ConversationListDto>> ListConversationsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            return context.Conversations.Where(x =>
                    x.ConversationMemberships.Any(m => m.MemberId == identityService.CurrentUserId))
                .ProjectTo<ConversationListDto>(mapper.ConfigurationProvider)
                .OrderByDescending(x => x.LastMessage.CreatedAt)
                .ToPagedListAsync(pageIndex, pageSize, cancellationToken);
        }

        public Task<ConversationDetailsDto> GetConversationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return context.Conversations.Where(x => x.Id == id)
                .ProjectTo<ConversationDetailsDto>(mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);
        }

        public async Task<ConversationDetailsDto> CreateConversationAsync(ConversationCreateEditDto dto, CancellationToken cancellationToken = default)
        {
            var user = await context.Users.FindByIdAsync(identityService.CurrentUserId, cancellationToken);
            var conversation = new Conversation
            {
                Name = dto.Name,
                ConversationMemberships = new List<ConversationMembership>
                {
                    new ConversationMembership
                    {
                        Member = user,
                        IsAdmin = true
                    }
                }
            };

            context.Conversations.Add(conversation);
            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<ConversationDetailsDto>(conversation);
        }

        public async Task<ConversationDetailsDto> UpdateConversationAsync(Guid id, ConversationCreateEditDto dto, CancellationToken cancellationToken = default)
        {
            var conversation = await context.Conversations.Include(x => x.ConversationMemberships)
                    .ThenInclude(x => x.Member)
                .FindByIdAsync(id, cancellationToken);

            if (!conversation.IsAdmin(identityService.CurrentUserId))
            {
                throw new UnauthorizedAccessException("Only the administrators can edit the conversation.");
            }

            conversation.Name = dto.Name;
            await context.SaveChangesAsync(cancellationToken);
            var resultDto = mapper.Map<ConversationDetailsDto>(conversation);

            await notificationService.Users(conversation.ConversationMemberships
                    .Where(x => x.MemberId != identityService.CurrentUserId)
                    .Select(x => x.MemberId))
                .ConversationUpdated(resultDto);

            return resultDto;
        }

        public async Task DeleteConversationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var conversation = await context.Conversations.Include(x => x.ConversationMemberships)
                .FindByIdAsync(id, cancellationToken);

            if (!conversation.IsAdmin(identityService.CurrentUserId))
            {
                throw new UnauthorizedAccessException("Only the administrators can delete the conversation.");
            }

            context.Conversations.Remove(conversation);
            await context.SaveChangesAsync(cancellationToken);

            await notificationService.Users(conversation.ConversationMemberships.Where(x => x.MemberId != identityService.CurrentUserId)
                    .Select(x => x.MemberId))
                .RemovedFromConversation(conversation.Id);
        }
    }
}