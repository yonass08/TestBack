using Application.Features.Services.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Services.CQRS.Queries;

public class GetServiceByNameQuery : IRequest<Result<ServiceDto>>
    {
        public string ServiceName { get; set; }
    }