using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Minigram.Application.Abstractions.Dtos;
using Minigram.Application.Abstractions.Services;
using Minigram.Application.Abstractions.Services.Notification;
using Minigram.Application.Extensions;
using Minigram.Application.Features.FriendManagement.Interface.Clients;
using Minigram.Application.Features.FriendManagement.Interface.Dtos;
using Minigram.Application.Features.FriendManagement.Interface.Services;
using Minigram.Dal;
using Minigram.Dal.Entities;
using Minigram.Dal.Extensions;

namespace Minigram.Application.Features.FriendManagement.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly MinigramDbContext context;
        private readonly IIdentityService identityService;
        private readonly IConfigurationProvider configurationProvider;
        private readonly IMapper mapper;
        private readonly INotificationService<IFriendRequestClient> notificationService;

        public FriendRequestService(
            MinigramDbContext context,
            IIdentityService identityService,
            IConfigurationProvider configurationProvider,
            IMapper mapper,
            INotificationService<IFriendRequestClient> notificationService)
        {
            this.context = context;
            this.identityService = identityService;
            this.configurationProvider = configurationProvider;
            this.mapper = mapper;
            this.notificationService = notificationService;
        }
        
        public Task<PagedListDto<FriendRequestDto>> ListSentRequestsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            return context.FriendRequests.Where(x => x.SenderId == identityService.CurrentUserId)
                .OrderBy(x => x.Recipient.UserName)
                .ProjectTo<FriendRequestDto>(configurationProvider)
                .ToPagedListAsync(pageIndex, pageSize, cancellationToken);
        }

        public Task<PagedListDto<FriendRequestDto>> ListReceivedRequestsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            return context.FriendRequests.Where(x => x.RecipientId == identityService.CurrentUserId)
                .OrderBy(x => x.Sender.UserName)
                .ProjectTo<FriendRequestDto>(configurationProvider)
                .ToPagedListAsync(pageIndex, pageSize, cancellationToken);
        }

        public async Task<FriendRequestDto> SendRequestAsync(Guid recipientId, CancellationToken cancellationToken = default)
        {
            var recipient = await context.Users.FindByIdAsync(recipientId, cancellationToken);
            var sender = await context.Users.FindByIdAsync(identityService.CurrentUserId, cancellationToken);

            var request = new FriendRequest
            {
                Recipient = recipient,
                Sender = sender
            };

            context.FriendRequests.Add(request);
            await context.SaveChangesAsync(cancellationToken);

            var dto = mapper.Map<FriendRequestDto>(request);
            
            await notificationService.NotifyUser(recipientId)
                .That(x => x.FriendRequestCreated(dto))
                .SendAsync();
            
            return dto;
        }

        public async Task DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            var request = await context.FriendRequests.FindByIdAsync(requestId, cancellationToken);
            if (identityService.CurrentUserId != request.RecipientId &&
                identityService.CurrentUserId != request.SenderId)
            {
                throw new UnauthorizedAccessException(
                    "Friend requests can only be deleted by their sender or recipient.");
            }

            context.FriendRequests.Remove(request);
            await context.SaveChangesAsync(cancellationToken);

            await notificationService.NotifyUser(identityService.CurrentUserId == request.SenderId
                    ? request.RecipientId
                    : request.SenderId)
                .That(x => x.FriendRequestDeleted(requestId))
                .SendAsync();
        }
    }
}