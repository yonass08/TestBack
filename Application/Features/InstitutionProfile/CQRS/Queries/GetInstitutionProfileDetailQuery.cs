
using MediatR;
using Application.Responses;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.InstitutionProfiles.CQRS.Queries
{
    public class GetInstitutionProfileDetailQuery : IRequest<Result<InstitutionProfileDetailDto>>
    {
        public Guid Id { get; set; }
    }
}