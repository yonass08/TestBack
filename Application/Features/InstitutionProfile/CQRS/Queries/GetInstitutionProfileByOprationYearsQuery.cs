
using MediatR;
using Application.Responses;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.InstitutionProfiles.CQRS.Queries
{
    public class GetInstitutionProfileByOprationYearsQuery : IRequest<Result<List<InstitutionProfileDto>>>
    {
        public int Years { get; set; }
    }
}