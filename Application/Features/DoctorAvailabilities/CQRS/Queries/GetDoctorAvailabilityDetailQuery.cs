
using MediatR;
using Application.Features.DoctorAvailabilities.DTOs;
using Application.Responses;

namespace Application.Features.DoctorAvailabilities.CQRS.Queries
{
    public class GetDoctorAvailabilityDetailQuery : IRequest<Result<DoctorAvailabilityDto>>
    {
        public Guid Id { get; set; }
    }
}