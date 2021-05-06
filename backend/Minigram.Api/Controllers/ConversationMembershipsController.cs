using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Minigram.Api.Resources.AuthorizationConstants.Scopes;
using Minigram.Application.Features.Conversations.Interface.Dtos;
using Minigram.Application.Features.Conversations.Interface.Services;

namespace Minigram.Api.Controllers
{
    [Route("api/conversations/{conversationId}/memberships/{membershipId}")]
    [Route("api/memberships/{membershipId}")]
    [ApiController]
    [Authorize(Conversations.Manage)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ConversationMembershipsController : ControllerBase
    {
        private readonly IConversationMemberService conversationMemberService;

        public ConversationMembershipsController(IConversationMemberService conversationMemberService)
        {
            this.conversationMemberService = conversationMemberService;
        }
        
        [HttpPut]
        [Description("Update the role of a member")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<ConversationMembershipDto> UpdateMemberAsync(
            [Description("The id of the conversation")] Guid conversationId,
            [Description("The id of the membership")] Guid membershipId,
            [Description("The updated membership")] [FromBody] ConversationMembershipEditDto dto,
            CancellationToken cancellationToken)
        {
            return conversationMemberService.EditMembershipAsync(membershipId, dto, cancellationToken);
        }

        [HttpDelete]
        [Description("Remove a member from the conversation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteMemberAsync(
            [Description("The id of the conversation")] Guid conversationId,
            [Description("The id of the membership")] Guid membershipId,
            CancellationToken cancellationToken)
        {
            await conversationMemberService.RemoveMembershipAsync(membershipId, cancellationToken);
            return NoContent();
        }
    }
}