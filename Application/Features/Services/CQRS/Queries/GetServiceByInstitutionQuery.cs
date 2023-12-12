using Application.Features.Services.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Services.CQRS.Queries;
public class GetServiceByInstitutionQuery : IRequest<Result<List<ServiceDto>>>
    {
        public Guid InstitutionId { get; set; }
    }

