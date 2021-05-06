using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minigram.Application.Abstractions.Dtos;
using Minigram.Application.Features.Conversations.Interface.Dtos;
using Minigram.Application.Features.Conversations.Interface.Services;
using static Minigram.Api.Resources.AuthorizationConstants.Scopes;

namespace Minigram.Api.Controllers
{
    [Route("api/conversations/{conversationId}/messages")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService messageService;

        public MessagesController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet]
        [Authorize(Conversations.Read)]
        [Description("List messages in the conversation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<PagedListDto<MessageDto>> ListMessagesAsync(Guid conversationId,
            [Description("The index of the page")] [FromQuery] int pageIndex = 0,
            [Description("The amount of items per page")] [FromQuery] int pageSize = 25,
            CancellationToken cancellationToken = default)
        {
            return messageService.ListMessagesAsync(conversationId, pageIndex, pageSize, cancellationToken);
        }

        [HttpPost]
        [Authorize(Conversations.Manage)]
        [Description("Send a new message to the conversation")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<MessageDto>> SendMessageAsync(
            [Description("The id of the conversation")] Guid conversationId,
            [Description("The content of the message")] [FromBody] string text,
            CancellationToken cancellationToken)
        {
            var message = await messageService.SendAsync(conversationId, text, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, message);
        }
    }
}