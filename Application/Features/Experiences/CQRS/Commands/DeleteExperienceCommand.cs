using Application.Responses;
using MediatR;

namespace Application.Features.Experiences.CQRS.Commands;

public class DeleteExperienceCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }
