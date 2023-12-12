using Application.Features.Services.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Services.CQRS.Commands;

public class CreateServiceCommand  : IRequest<Result<Guid>>
    {
        public CreateServiceDto ServiceDto { get; set; }
    }

