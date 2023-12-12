using MediatR;
using Application.Features.InstitutionAvailabilities.DTOs;
using Application.Responses;

namespace Application.Features.InstitutionAvailabilities.CQRS.Commands
{
    public class UpdateInstitutionAvailabilityCommand : IRequest<Result<Unit>>
    {
        public UpdateInstitutionAvailabilityDto UpdateInstitutionAvailabilityDto { get; set; }

    }
}

