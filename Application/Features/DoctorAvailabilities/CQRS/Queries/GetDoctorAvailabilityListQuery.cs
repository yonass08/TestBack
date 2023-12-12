
using MediatR;
using Application.Features.DoctorAvailabilities.DTOs;
using Application.Responses;

namespace Application.Features.DoctorAvailabilities.CQRS.Queries
{
    public class GetDoctorAvailabilityListQuery : IRequest<Result<List<DoctorAvailabilityDto>>>

    {

    }
}