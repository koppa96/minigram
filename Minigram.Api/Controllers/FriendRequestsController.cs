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
    [Route("api/friend-requests")]
    [ApiController]
    public class FriendRequestsController : ControllerBase
    {
        private readonly IFriendRequestService friendRequestService;

        public FriendRequestsController(IFriendRequestService friendRequestService)
        {
            this.friendRequestService = friendRequestService;
        }

        [HttpGet("sent")]
        [Authorize(Friendships.Read)]
        public Task<PagedListDto<FriendRequestDto>> ListSentRequestsAsync(int pageIndex = 0, int pageSize = 25,
            CancellationToken cancellationToken = default)
        {
            return friendRequestService.ListSentRequestsAsync(pageIndex, pageSize, cancellationToken);
        }

        [HttpGet("received")]
        [Authorize(Friendships.Read)]
        public Task<PagedListDto<FriendRequestDto>> ListReceivedRequestsAsync(int pageIndex = 0, int pageSize = 25,
            CancellationToken cancellationToken = default)
        {
            return friendRequestService.ListReceivedRequestsAsync(pageIndex, pageSize, cancellationToken);
        }

        [HttpPost]
        [Authorize(Friendships.Manage)]
        public async Task<ActionResult<FriendRequestDto>> SendRequestAsync([FromBody] FriendRequestCreateDto dto,
            CancellationToken cancellationToken)
        {
            var request = await friendRequestService.SendRequestAsync(dto.RecipientId, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, request);
        }

        [HttpDelete("{requestId}")]
        [Authorize(Friendships.Manage)]
        public async Task<ActionResult> DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken)
        {
            await friendRequestService.DeleteRequestAsync(requestId, cancellationToken);
            return NoContent();
        }
    }
}