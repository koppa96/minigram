using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minigram.Application.Abstractions.Dtos;
using Minigram.Application.Abstractions.Services;
using Minigram.Application.Abstractions.Services.Notification;
using Minigram.Application.Extensions;
using Minigram.Application.Features.FriendManagement.Interface.Clients;
using Minigram.Application.Features.FriendManagement.Interface.Dtos;
using Minigram.Application.Features.FriendManagement.Interface.Services;
using Minigram.Application.Features.Users.Dtos;
using Minigram.Dal;
using Minigram.Dal.Entities;
using Minigram.Dal.Extensions;

namespace Minigram.Application.Features.FriendManagement.Services
{
    public class FriendService : IFriendService
    {
        private readonly MinigramDbContext context;
        private readonly IIdentityService identityService;
        private readonly INotificationService<IFriendshipClient> notificationService;

        public FriendService(
            MinigramDbContext context,
            IIdentityService identityService,
            INotificationService<IFriendshipClient> notificationService)
        {
            this.context = context;
            this.identityService = identityService;
            this.notificationService = notificationService;
        }
        
        public Task<PagedListDto<FriendshipDto>> ListFriendshipsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            return context.Friendships.Where(x => x.User1Id == identityService.CurrentUserId || x.User2Id == identityService.CurrentUserId)
                .Select(x => new FriendshipDto
                {
                    Id = x.Id,
                    Friend = new UserListDto
                    {
                        Id = x.User1Id == identityService.CurrentUserId ? x.User2Id : x.User1Id,
                        UserName = x.User1Id == identityService.CurrentUserId ? x.User2.UserName : x.User1.UserName
                    }
                })
                .OrderBy(x => x.Friend.UserName)
                .ToPagedListAsync(pageIndex, pageSize, cancellationToken);
        }

        public async Task<FriendshipDto> CreateFriendshipAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            var request = await context.FriendRequests.Include(x => x.Sender)
                .Include(x => x.Recipient)
                .FindByIdAsync(requestId, cancellationToken);

            if (request.RecipientId != identityService.CurrentUserId)
            {
                throw new UnauthorizedAccessException("The friend request can only be accepted by its recipient.");
            }

            var friendship = new Friendship
            {
                User1 = request.Sender,
                User2 = request.Recipient
            };

            context.Friendships.Add(friendship);
            context.FriendRequests.Remove(request);
            await context.SaveChangesAsync(cancellationToken);

            await notificationService.NotifyUser(request.SenderId)
                .That(x => x.FriendshipCreated(new FriendshipDto
                {
                    Id = friendship.Id,
                    Friend = new UserListDto
                    {
                        Id = request.RecipientId,
                        UserName = request.Recipient.UserName
                    }
                }))
                .SendAsync();
            
            return new FriendshipDto
            {
                Id = friendship.Id,
                Friend = new UserListDto
                {
                    Id = request.SenderId,
                    UserName = request.Sender.UserName
                }
            };
        }

        public async Task DeleteFriendship(Guid friendshipId, CancellationToken cancellationToken = default)
        {
            var friendship = await context.Friendships.FindByIdAsync(friendshipId, cancellationToken);
            if (friendship.User1Id != identityService.CurrentUserId &&
                friendship.User2Id != identityService.CurrentUserId)
            {
                throw new UnauthorizedAccessException("Only the members of a friendship can delete a friendship.");
            }
            
            context.Friendships.Remove(friendship);
            await context.SaveChangesAsync(cancellationToken);

            await notificationService.NotifyUser(friendship.User1Id == identityService.CurrentUserId
                    ? friendship.User2Id
                    : friendship.User1Id)
                .That(x => x.FriendshipDeleted(friendshipId))
                .SendAsync();
        }
    }
}