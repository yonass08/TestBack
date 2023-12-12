using Application.Contracts.Persistence;
using Application.Features.Educations.CQRS.Queries;
using Application.Features.Educations.DTOs;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.Educations.CQRS.Handlers;

public class GetEducationInstitutionNameAndLogoQueryHandler : IRequestHandler<GetEducationInstitutionNameAndLogoQuery, Result<List<GetEducationInstitutionNameAndLogoDto>>>{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

public GetEducationInstitutionNameAndLogoQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
{
    _unitOfWork = unitOfWork;
    _mapper = mapper;
}

public async Task<Result<List<GetEducationInstitutionNameAndLogoDto>>> Handle(GetEducationInstitutionNameAndLogoQuery request, CancellationToken cancellationToken)
{
    var education = await _unitOfWork.EducationRepository.GetAll();
    var response = new Result<List<GetEducationInstitutionNameAndLogoDto>>();
    if (education == null)
    {
        response.Value = null;
        response.IsSuccess = false;
        response.Error = "Fetch Failed";
    }
    else
    {
        var educationMapped = _mapper.Map<List<GetEducationInstitutionNameAndLogoDto>>(education);
        response.Value = educationMapped;
        response.IsSuccess = true;
        response.Error = "Fetch Succesful";
    }
    return response;
}
}
