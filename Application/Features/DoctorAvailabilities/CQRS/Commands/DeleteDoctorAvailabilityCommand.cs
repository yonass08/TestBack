using MediatR;
using Application.Responses;

namespace Application.Features.DoctorAvailabilities.CQRS.Commands
{
    public class DeleteDoctorAvailabilityCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }
}
