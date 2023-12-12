
using MediatR;
using Application.Features.Specialities.DTOs;
using Application.Responses;

namespace Application.Features.Specialities.CQRS.Queries
{
    public class GetSpecialityListQuery : IRequest<Result<List<SpecialityDto>>>

    {

    }
}