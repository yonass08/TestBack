
using MediatR;
using Application.Responses;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.InstitutionProfiles.CQRS.Queries
{
    public class GetInstitutionProfileByServiceQuery : IRequest<Result<List<InstitutionProfileDto>>>
    {
        public Guid ServiceId { get; set; }
    }
}