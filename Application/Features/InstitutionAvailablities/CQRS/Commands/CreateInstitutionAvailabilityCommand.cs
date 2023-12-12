using MediatR;
using Application.Responses;
using Application.Features.InstitutionAvailabilities.DTOs;

namespace Application.Features.InstitutionAvailabilities.CQRS.Commands
{
    public class CreateInstitutionAvailabilityCommand : IRequest<Result<Guid>>
    {
        public CreateInstitutionAvailabilityDto CreateInstitutionAvailabilityDto { get; set; }
    }
}
