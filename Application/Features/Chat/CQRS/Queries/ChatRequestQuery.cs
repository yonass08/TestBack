using Application.Features.Chat.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Chat.CQRS.Queries
{
    public class ChatRequestQuery : IRequest<Result<ChatResponseDto>>
    {
        public ChatRequestDto ChatRequestDto {get; set;}
    }
}
