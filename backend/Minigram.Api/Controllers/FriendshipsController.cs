using System;
using System.ComponentModel;
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
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class FriendshipsController : ControllerBase
    {
        private readonly IFriendService friendService;

        public FriendshipsController(IFriendService friendService)
        {
            this.friendService = friendService;
        }

        [HttpGet]
        [Authorize(Friendships.Read)]
        [Description("List friends")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<PagedListDto<FriendshipDto>> ListFriendsAsync(
            [Description("The name of the friend")] [FromQuery] string searchText = null,
            [Description("The index of the page")] [FromQuery] int pageIndex = 0,
            [Description("The amount of items per page")] [FromQuery] int pageSize = 25,
            CancellationToken cancellationToken = default)
        {
            return friendService.ListFriendshipsAsync(searchText, pageIndex, pageSize, cancellationToken);
        }

        [HttpPost]
        [Authorize(Friendships.Manage)]
        [Description("Accept a friend request")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<FriendshipDto>> CreateFriendshipAsync(
            [Description("The details of the friendship")] [FromBody] FriendshipCreateDto dto,
            CancellationToken cancellationToken)
        {
            var friendship = await friendService.CreateFriendshipAsync(dto.RequestId, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, friendship);
        }

        [HttpDelete("{friendshipId}")]
        [Authorize(Friendships.Manage)]
        [Description("Delete a friendship")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteFriendshipAsync(
            [Description("The id of the friendship")] Guid friendshipId,
            CancellationToken cancellationToken)
        {
            await friendService.DeleteFriendshipAsync(friendshipId, cancellationToken);
            return NoContent();
        }
    }
}