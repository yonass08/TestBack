using Application.Features.Educations.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Educations.CQRS;

public class UpdateEducationCommand : IRequest<Result<Unit?>>
{
    public UpdateEducationDto updateEducationDto {get;set;}
}
