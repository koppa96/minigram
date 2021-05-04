using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minigram.Application.Abstractions.Dtos;
using Minigram.Application.Features.Conversations.Interface.Dtos;
using Minigram.Application.Features.Conversations.Interface.Services;

namespace Minigram.Api.Controllers
{
    [Route("api/conversations/{conversationId}/messages")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService messageService;

        public MessagesController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet]
        public Task<PagedListDto<MessageDto>> ListMessagesAsync(Guid conversationId, int pageIndex = 0, int pageSize = 0,
            CancellationToken cancellationToken = default)
        {
            return messageService.ListMessagesAsync(conversationId, pageIndex, pageSize, cancellationToken);
        }

        [HttpPost]
        public Task<MessageDto> SendMessageAsync(Guid conversationId, [FromBody] string text,
            CancellationToken cancellationToken)
        {
            return messageService.SendAsync(conversationId, text, cancellationToken);
        }
    }
}