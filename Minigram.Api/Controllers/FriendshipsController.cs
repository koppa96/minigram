using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minigram.Application.Abstractions.Dtos;
using Minigram.Application.Features.FriendManagement.Interface.Dtos;
using Minigram.Application.Features.FriendManagement.Interface.Services;
using static Minigram.Api.Resources.AuthorizationConstants.Scopes;

namespace Minigram.Api.Controllers
{
    [Route("api/friendships")]
    [ApiController]
    public class FriendshipsController : ControllerBase
    {
        private readonly IFriendService friendService;

        public FriendshipsController(IFriendService friendService)
        {
            this.friendService = friendService;
        }

        [HttpGet]
        [Authorize(Friendships.Read)]
        public Task<PagedListDto<FriendshipDto>> ListFriendsAsync(int pageIndex = 0, int pageSize = 25,
            CancellationToken cancellationToken = default)
        {
            return friendService.ListFriendshipsAsync(pageIndex, pageSize, cancellationToken);
        }

        [HttpPost]
        [Authorize(Friendships.Manage)]
        public async Task<ActionResult<FriendshipDto>> CreateFriendshipAsync([FromBody] FriendshipCreateDto dto,
            CancellationToken cancellationToken)
        {
            var friendship = await friendService.CreateFriendshipAsync(dto.RequestId, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, friendship);
        }

        [HttpDelete("{friendshipId}")]
        [Authorize(Friendships.Manage)]
        public async Task<ActionResult> DeleteFriendshipAsync(Guid friendshipId, CancellationToken cancellationToken)
        {
            await friendService.DeleteFriendshipAsync(friendshipId, cancellationToken);
            return NoContent();
        }
    }
}