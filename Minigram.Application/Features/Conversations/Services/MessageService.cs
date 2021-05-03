using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Minigram.Application.Abstractions.Dtos;
using Minigram.Application.Abstractions.Services;
using Minigram.Application.Extensions;
using Minigram.Application.Features.Conversations.Interface.Dtos;
using Minigram.Application.Features.Conversations.Interface.Services;
using Minigram.Dal;
using Minigram.Dal.Entities;

namespace Minigram.Application.Features.Conversations.Services
{
    public class MessageService : IMessageService
    {
        private readonly MinigramDbContext context;
        private readonly IIdentityService identityService;
        private readonly IMapper mapper;

        public MessageService(MinigramDbContext context, IIdentityService identityService, IMapper mapper)
        {
            this.context = context;
            this.identityService = identityService;
            this.mapper = mapper;
        }

        public Task<PagedListDto<MessageDto>> ListMessagesAsync(Guid conversationId, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            return context.Messages.Where(x => x.ConversationMembership.ConversationId == conversationId)
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
                .ToPagedListAsync(pageIndex, pageSize, cancellationToken);
        }

        public async Task<MessageDto> SendAsync(Guid conversationId, string text,
            CancellationToken cancellationToken = default)
        {
            var membership = await context.ConversationMemberships.Include(x => x.Member)
                .SingleAsync(x => x.ConversationId == conversationId &&
                                  x.MemberId == identityService.CurrentUserId, cancellationToken);

            var message = new Message
            {
                Text = text,
                ConversationMembership = membership,
                CreatedAt = DateTime.UtcNow
            };

            context.Messages.Add(message);
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<MessageDto>(message);
        }
    }
}