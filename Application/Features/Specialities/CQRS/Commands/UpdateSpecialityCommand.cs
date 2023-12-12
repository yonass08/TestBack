
using MediatR;
using Application.Features.Specialities.DTOs;
using Application.Responses;

namespace Application.Features.Specialities.CQRS.Commands
{
    public class UpdateSpecialityCommand : IRequest<Result<Unit?>>
    {
        public UpdateSpecialityDto SpecialityDto { get; set; }

    }
}
