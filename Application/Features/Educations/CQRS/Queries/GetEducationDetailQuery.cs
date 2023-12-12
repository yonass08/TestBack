using Application.Features.Educations.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Educations.CQRS.Queries;

public class GetEducationDetailQuery : IRequest<Result<EducationDto>>
{
    public Guid Id { get; set; }
}
