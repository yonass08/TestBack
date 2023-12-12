using Application.Features.Experiences.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Experiences.CQRS.Commands;

public class CreateExperienceCommand : IRequest<Result<Guid>>
    {
        public CreateExperienceDto ExperienceDto { get; set; }
    }
