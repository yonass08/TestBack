using Application.Features.Educations.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Educations.CQRS.Queries;

public class GetEducationListQuery : IRequest<Result<List<EducationDto>>>
{
    
}
