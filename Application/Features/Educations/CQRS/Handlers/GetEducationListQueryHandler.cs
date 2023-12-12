using Application.Contracts.Persistence;
using Application.Features.Educations.CQRS.Queries;
using Application.Features.Educations.DTOs;
using Application.Responses;
using AutoMapper;
using MediatR;
namespace Application.Features.Educations.CQRS.Handlers;

public class GetEducationListQueryHandler : IRequestHandler<GetEducationListQuery, Result<List<EducationDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEducationListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<EducationDto>>> Handle(GetEducationListQuery request, CancellationToken cancellationToken)
    {
        var education = await _unitOfWork.EducationRepository.GetAllPopulated();
        var response = new Result<List<EducationDto>>();
        if (education == null)
        { 
            response.Value = null;
            response.IsSuccess = false;
            response.Error = "Fetch Failed";
        }else{
            var educationMapped = _mapper.Map<List<EducationDto>>(education);
            response.Value = educationMapped;
            response.IsSuccess = true;
            response.Error = "Fetch Succesful";
        }
        return response;
    }
}
