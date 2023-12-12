
using MediatR;
using Application.Responses;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.Specialities.CQRS.Queries
{
    public class GetInstitutionProfileListQuery : IRequest<Result<List<InstitutionProfileDto>>>

    {

    }
}