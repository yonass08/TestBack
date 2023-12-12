
using MediatR;
using Application.Features.InstitutionAvailabilities.DTOs;
using Application.Responses;

namespace Application.Features.InstitutionAvailabilities.CQRS.Queries
{
    public class GetInstitutionAvailabilityDetailQuery : IRequest<Result<InstitutionAvailabilityDto>>
    {
        public Guid Id { get; set; }
    }
}