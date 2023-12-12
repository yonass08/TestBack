using MediatR;
using Application.Responses;
using Application.Features.DoctorAvailabilities.DTOs;

namespace Application.Features.DoctorAvailabilities.CQRS.Commands
{
    public class CreateDoctorAvailabilityCommand : IRequest<Result<Guid>>
    {
        public CreateDoctorAvailabilityDto CreateDoctorAvailabilityDto { get; set; }
    }
}
