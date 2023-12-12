using MediatR;
using Application.Responses;

namespace Application.Features.InstitutionAvailabilities.CQRS.Commands
{
    public class DeleteInstitutionAvailabilityCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }
}
