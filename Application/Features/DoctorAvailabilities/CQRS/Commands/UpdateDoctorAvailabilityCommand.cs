
using MediatR;
using Application.Features.DoctorAvailabilities.DTOs;
using Application.Responses;

namespace Application.Features.DoctorAvailabilities.CQRS.Commands
{
    public class UpdateDoctorAvailabilityCommand : IRequest<Result<Unit>>
    {
        public UpdateDoctorAvailabilityDto UpdateDoctorAvailabilityDto { get; set; }

    }
}
