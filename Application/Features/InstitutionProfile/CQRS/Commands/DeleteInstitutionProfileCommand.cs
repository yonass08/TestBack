using MediatR;
using Application.Responses;

namespace Application.Features.InstitutionProfiles.CQRS.Commands
{
    public class DeleteInstitutionProfileCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }
}
