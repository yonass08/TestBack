using Application.Features.Experiences.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Experiences.CQRS.Commands;

public class UpdateExperienceCommand : IRequest<Result<Unit>>
    {
        public UpdateExperienceDto ExperienceDto { get; set; }

    }


