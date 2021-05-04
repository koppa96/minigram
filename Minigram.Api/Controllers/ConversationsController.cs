using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Minigram.Api.Resources.AuthorizationConstants.Scopes;
using Minigram.Application.Abstractions.Dtos;
using Minigram.Application.Features.Conversations.Interface.Dtos;
using Minigram.Application.Features.Conversations.Interface.Services;

namespace Minigram.Api.Controllers
{
    [Route("api/conversations")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService conversationService;
        private readonly IConversationMemberService conversationMemberService;

        public ConversationsController(IConversationService conversationService, IConversationMemberService conversationMemberService)
        {
            this.conversationService = conversationService;
            this.conversationMemberService = conversationMemberService;
        }

        [HttpGet]
        [Authorize(Conversations.Read)]
        public Task<PagedListDto<ConversationListDto>> ListConversationsAsync([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 25,
            CancellationToken cancellationToken = default)
        {
            return conversationService.ListConversationsAsync(pageIndex, pageSize, cancellationToken);
        }

        [HttpGet("{conversationId}")]
        [Authorize(Conversations.Read)]
        public Task<ConversationDetailsDto> GetConversationDetailsAsync(Guid conversationId, CancellationToken cancellationToken)
        {
            return conversationService.GetConversationAsync(conversationId, cancellationToken);
        }

        [HttpPost]
        [Authorize(Conversations.Manage)]
        public async Task<ActionResult<ConversationDetailsDto>> CreateConversationAsync(
            [FromBody] ConversationCreateEditDto dto, CancellationToken cancellationToken)
        {
            var conversation = await conversationService.CreateConversationAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetConversationDetailsAsync), new { conversationId = conversation.Id }, conversation);
        }
        
        [HttpPost]
        [Authorize(Conversations.Manage)]
        public async Task<ActionResult<ConversationMembershipDto>> AddMemberAsync(Guid conversationId,
            [FromBody] ConversationMembershipCreateDto dto, CancellationToken cancellationToken)
        {
            var membership = await conversationMemberService.AddMembershipAsync(conversationId, dto.UserId, cancellationToken);
            
            // This controller has no get endpoint so no location header value is returned
            return StatusCode(StatusCodes.Status201Created, membership);
        }

        [HttpPut("{conversationId}")]
        [Authorize(Conversations.Manage)]
        public Task<ConversationDetailsDto> UpdateConversationAsync(Guid conversationId,
            [FromBody] ConversationCreateEditDto dto, CancellationToken cancellationToken)
        {
            return conversationService.UpdateConversationAsync(conversationId, dto, cancellationToken);
        }

        [HttpDelete("{conversationId}")]
        [Authorize(Conversations.Manage)]
        public async Task<ActionResult> DeleteConversationAsync(Guid conversationId, CancellationToken cancellationToken)
        {
            await conversationService.DeleteConversationAsync(conversationId, cancellationToken);
            return NoContent();
        }
    }
}