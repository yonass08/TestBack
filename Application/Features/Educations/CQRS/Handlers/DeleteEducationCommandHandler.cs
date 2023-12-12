

using System.Net;
using AutoMapper;
using Application.Contracts.Persistence;
using Application.Responses;
using MediatR;
namespace Application.Features.Educations.CQRS.Handlers;

public class DeleteEducationCommandHandler: IRequestHandler<DeleteEducationCommand, Result<Guid?>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteEducationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Guid?>> Handle(DeleteEducationCommand request, CancellationToken cancellationToken)
    {
        var education = await _unitOfWork.EducationRepository.Get(request.Id);
        var response = new Result<Guid?>();
        if (education is null){
            response.IsSuccess = false;
            response.Value = null;
            response.Error = "Education Not Found.";
            return response;
        }
        await _unitOfWork.EducationRepository.Delete(education);
        if (await _unitOfWork.Save() > 0){
            response.IsSuccess = true;
            response.Value = education.Id;
            response.Error = "Education Deleted Successfully.";
        }else{
            response.IsSuccess = false;
            response.Value = null;
            response.Error = "Education Deletion Failed.";
        }
        return response;
    }
}
