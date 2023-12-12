using Application.Features.Chat.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Chat.CQRS.Queries;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class ChatController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequestDto requestDto)
        {
            return HandleResult(await _mediator.Send(new ChatRequestQuery{ChatRequestDto = requestDto}));
        }
    }
};