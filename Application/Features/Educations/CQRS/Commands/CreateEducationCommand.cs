using Application.Features.Educations.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Educations.CQRS;

public class CreateEducationCommand  : IRequest<Result<CreateEducationDto>>
{
    public CreateEducationDto createEducationDto {get;set;}
}
