using MediatR;
using Application.Responses;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.InstitutionProfiles.CQRS.Commands
{
    public class CreateInstitutionProfileCommand : IRequest<Result<Guid>>
    {
        public CreateInstitutionProfileDto CreateInstitutionProfileDto { get; set; }
    }
}
