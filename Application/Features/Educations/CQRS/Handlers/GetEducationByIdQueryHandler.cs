using Application.Contracts.Persistence;
using Application.Features.Educations.CQRS.Queries;
using Application.Features.Educations.DTOs;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.Educations.CQRS.Handlers;

public class GetEducationByIdQueryHandler : IRequestHandler<GetEducationDetailQuery, Result<EducationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEducationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<EducationDto>> Handle(GetEducationDetailQuery request, CancellationToken cancellationToken)
    {
        var education = await _unitOfWork.EducationRepository.GetPopulated(request.Id);
        var response = new Result<EducationDto>();
        if (education == null)
        {
            response.Value = null;
            response.IsSuccess = false;
            response.Error = "Fetch Failed";
        }
        else
        {
            var educationMapped = _mapper.Map<EducationDto>(education);
            response.Value = educationMapped;
            response.IsSuccess = true;
            response.Error = "Fetch Succesful";
        }
        return response;
    }
}

