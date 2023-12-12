using Application.Features.Educations.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Educations.CQRS.Queries;

public class GetEducationInstitutionNameAndLogoQuery: IRequest<Result<List<GetEducationInstitutionNameAndLogoDto>>>
{

}
