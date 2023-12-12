
using Application.Features.DoctorProfiles.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.DoctorProfiles.CQRS.Queris
{
    public class GetDoctorProfileListQuery : IRequest<Result<List<DoctorProfileDto>>>
    {
    }
}