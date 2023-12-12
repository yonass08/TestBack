using Application.Features.Services.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Services.CQRS.Commands;

public class UpdateServiceCommand : IRequest<Result<Unit>>
    {
        public UpdateServiceDto ServiceDto { get; set; }

    }
