using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minigram.Application.Features.Conversations.Interface.Dtos;
using Minigram.Application.Features.Conversations.Interface.Services;

namespace Minigram.Api.Controllers
{
    [Route("api/conversations/{conversationId}/memberships/{membershipId}")]
    [Route("api/memberships/{membershipId}")]
    [ApiController]
    [Authorize]
    public class ConversationMembershipsController : ControllerBase
    {
        private readonly IConversationMemberService conversationMemberService;

        public ConversationMembershipsController(IConversationMemberService conversationMemberService)
        {
            this.conversationMemberService = conversationMemberService;
        }
        
        [HttpPut]
        public Task<ConversationMembershipDto> UpdateMemberAsync(Guid conversationId, Guid membershipId,
            [FromBody] ConversationMembershipEditDto dto, CancellationToken cancellationToken)
        {
            return conversationMemberService.EditMembershipAsync(membershipId, dto, cancellationToken);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteMemberAsync(Guid conversationId, Guid membershipId, CancellationToken cancellationToken)
        {
            await conversationMemberService.RemoveMembershipAsync(membershipId, cancellationToken);
            return NoContent();
        }
    }
}