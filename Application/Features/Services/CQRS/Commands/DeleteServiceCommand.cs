using Application.Responses;
using MediatR;

namespace Application.Features.Services.CQRS.Commands;

public class DeleteServiceCommand  : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }

