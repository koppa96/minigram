using System;
using System.ComponentModel;
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
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [Description("List the conversations the user is currently the member of")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<PagedListDto<ConversationListDto>> ListConversationsAsync(
            [Description("The index of the page")] [FromQuery] int pageIndex = 0,
            [Description("The amount of items per page")] [FromQuery] int pageSize = 25,
            CancellationToken cancellationToken = default)
        {
            return conversationService.ListConversationsAsync(pageIndex, pageSize, cancellationToken);
        }

        [HttpGet("{conversationId}")]
        [Authorize(Conversations.Read)]
        [Description("View the details of a specific conversation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<ConversationDetailsDto> GetConversationDetailsAsync(
            [Description("The id of the conversation")] Guid conversationId,
            CancellationToken cancellationToken)
        {
            return conversationService.GetConversationAsync(conversationId, cancellationToken);
        }

        [HttpPost]
        [Authorize(Conversations.Manage)]
        [Description("Create a new conversation")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ConversationDetailsDto>> CreateConversationAsync(
            [Description("The details of the conversation")] [FromBody] ConversationCreateEditDto dto,
            CancellationToken cancellationToken)
        {
            var conversation = await conversationService.CreateConversationAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetConversationDetailsAsync), new { conversationId = conversation.Id }, conversation);
        }
        
        [HttpPost("members")]
        [Authorize(Conversations.Manage)]
        [Description("Add a member to the conversation")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ConversationMembershipDto>> AddMemberAsync(
            [Description("The id of the conversation")] Guid conversationId,
            [Description("The details of the membership")] [FromBody] ConversationMembershipCreateDto dto,
            CancellationToken cancellationToken)
        {
            var membership = await conversationMemberService.AddMembershipAsync(conversationId, dto.UserId, cancellationToken);
            
            // This controller has no get endpoint so no location header value is returned
            return StatusCode(StatusCodes.Status201Created, membership);
        }

        [HttpPut("{conversationId}")]
        [Authorize(Conversations.Manage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<ConversationDetailsDto> UpdateConversationAsync(
            [Description("The id of the conversation")] Guid conversationId,
            [Description("The details of the conversation")] [FromBody] ConversationCreateEditDto dto,
            CancellationToken cancellationToken)
        {
            return conversationService.UpdateConversationAsync(conversationId, dto, cancellationToken);
        }

        [HttpDelete("{conversationId}")]
        [Authorize(Conversations.Manage)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteConversationAsync(
            [Description("The id of the conversation")] Guid conversationId,
            CancellationToken cancellationToken)
        {
            await conversationService.DeleteConversationAsync(conversationId, cancellationToken);
            return NoContent();
        }
    }
}