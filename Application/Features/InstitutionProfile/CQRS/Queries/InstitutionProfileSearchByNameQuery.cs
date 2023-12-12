
using MediatR;
using Application.Responses;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.Specialities.CQRS.Queries
{
    public class InstitutionProfileSearchByNameQuery: IRequest<Result<List<InstitutionProfileDto>>>
    {
        public string Name { get; set; } 
    }
}