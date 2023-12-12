using Application.Responses;
using MediatR;

namespace Application.Features.Educations.CQRS;

public class DeleteEducationCommand: IRequest<Result<Guid?>>
{
    public Guid Id { get; set; }
}
