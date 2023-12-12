using MediatR;
using Application.Responses;

namespace Application.Features.Specialities.CQRS.Commands
{
    public class DeleteSpecialityCommand : IRequest<Result<Guid?>>
    {
        public Guid Id { get; set; }
    }
}
