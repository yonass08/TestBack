
using MediatR;
using Application.Features.Specialities.DTOs;
using Application.Responses;

namespace Application.Features.Specialities.CQRS.Queries
{
    public class GetSpecialityDetailQuery : IRequest<Result<SpecialityDto>>
    {
        public Guid Id { get; set; }
    }
}