
using MediatR;
using Application.Features.InstitutionAvailabilities.DTOs;
using Application.Responses;

namespace Application.Features.InstitutionAvailabilities.CQRS.Queries
{
    public class GetInstitutionAvailabilityListQuery : IRequest<Result<List<InstitutionAvailabilityDto>>>

    {

    }
}