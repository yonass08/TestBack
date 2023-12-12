
using MediatR;
using Application.Responses;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.InstitutionProfiles.CQRS.Commands
{
    public class UpdateInstitutionProfileCommand : IRequest<Result<Unit>>
    {
        public UpdateInstitutionProfileDto UpdateInstitutionProfileDto { get; set; }

    }
}
