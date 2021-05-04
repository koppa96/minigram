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
    [Route("api/friend-requests")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class FriendRequestsController : ControllerBase
    {
        private readonly IFriendRequestService friendRequestService;

        public FriendRequestsController(IFriendRequestService friendRequestService)
        {
            this.friendRequestService = friendRequestService;
        }

        [HttpGet("sent")]
        [Authorize(Friendships.Read)]
        [Description("List sent friend requests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<PagedListDto<FriendRequestDto>> ListSentRequestsAsync(
            [Description("The index of the page")] [FromQuery] int pageIndex = 0,
            [Description("The amount of items per page")] [FromQuery] int pageSize = 25,
            CancellationToken cancellationToken = default)
        {
            return friendRequestService.ListSentRequestsAsync(pageIndex, pageSize, cancellationToken);
        }

        [HttpGet("received")]
        [Authorize(Friendships.Read)]
        [Description("List received friend requests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<PagedListDto<FriendRequestDto>> ListReceivedRequestsAsync(
            [Description("The index of the page")] [FromQuery] int pageIndex = 0,
            [Description("The amount of items per page")] [FromQuery] int pageSize = 25,
            CancellationToken cancellationToken = default)
        {
            return friendRequestService.ListReceivedRequestsAsync(pageIndex, pageSize, cancellationToken);
        }

        [HttpPost]
        [Authorize(Friendships.Manage)]
        [Description("Send a new friend request")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<FriendRequestDto>> SendRequestAsync(
            [Description("The details of the friend request")] [FromBody] FriendRequestCreateDto dto,
            CancellationToken cancellationToken)
        {
            var request = await friendRequestService.SendRequestAsync(dto.RecipientId, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, request);
        }

        [HttpDelete("{requestId}")]
        [Authorize(Friendships.Manage)]
        [Description("Delete a friend request")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteRequestAsync(
            [Description("The id of the friend request")] Guid requestId,
            CancellationToken cancellationToken)
        {
            await friendRequestService.DeleteRequestAsync(requestId, cancellationToken);
            return NoContent();
        }
    }
}