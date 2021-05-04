using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minigram.Application.Abstractions.Dtos;
using Minigram.Application.Features.Conversations.Interface.Dtos;
using Minigram.Application.Features.Conversations.Interface.Services;

namespace Minigram.Api.Controllers
{
    [Route("api/conversations")]
    [ApiController]
    [Authorize]
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
        public Task<PagedListDto<ConversationListDto>> ListConversationsAsync([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 25,
            CancellationToken cancellationToken = default)
        {
            return conversationService.ListConversationsAsync(pageIndex, pageSize, cancellationToken);
        }

        [HttpGet("{conversationId}")]
        public Task<ConversationDetailsDto> GetConversationDetailsAsync(Guid conversationId, CancellationToken cancellationToken)
        {
            return conversationService.GetConversationAsync(conversationId, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<ConversationDetailsDto>> CreateConversationAsync(
            [FromBody] ConversationCreateEditDto dto, CancellationToken cancellationToken)
        {
            var conversation = await conversationService.CreateConversationAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetConversationDetailsAsync), new { conversationId = conversation.Id }, conversation);
        }
        
        [HttpPost]
        public async Task<ActionResult<ConversationMembershipDto>> AddMemberAsync(Guid conversationId,
            [FromBody] ConversationMembershipCreateDto dto, CancellationToken cancellationToken)
        {
            var membership = await conversationMemberService.AddMembershipAsync(conversationId, dto.UserId, cancellationToken);
            
            // This controller has no get endpoint so no location header value is returned
            return StatusCode(StatusCodes.Status201Created, membership);
        }

        [HttpPut("{conversationId}")]
        public Task<ConversationDetailsDto> UpdateConversationAsync(Guid conversationId,
            [FromBody] ConversationCreateEditDto dto, CancellationToken cancellationToken)
        {
            return conversationService.UpdateConversationAsync(conversationId, dto, cancellationToken);
        }

        [HttpDelete("{conversationId}")]
        public async Task<ActionResult> DeleteConversationAsync(Guid conversationId, CancellationToken cancellationToken)
        {
            await conversationService.DeleteConversationAsync(conversationId, cancellationToken);
            return NoContent();
        }
    }
}