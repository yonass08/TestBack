
using AutoMapper;
using Application.Contracts.Persistence;
using Application.Responses;
using MediatR;
using Domain;
using Application.Features.Educations.DTOs;
using Application.Features.Educations.DTOs.Validators;
using Application.Interfaces;

namespace Application.Features.Educations.CQRS.Handlers;

public class UpdateEducationCommandHandler: IRequestHandler<UpdateEducationCommand, Result<Unit?>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPhotoAccessor _photoAccessor;

    public UpdateEducationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,IPhotoAccessor photoAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _photoAccessor = photoAccessor;

    }

    public async Task<Result<Unit?>> Handle(UpdateEducationCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateEducationDtoValidator();
        var validationResult = await validator.ValidateAsync(request.updateEducationDto);

        if (!validationResult.IsValid)
            return Result<Unit?>.Failure(validationResult.Errors[0].ErrorMessage);


        //................
        var response = new Result<Unit?>();
        var education = await _unitOfWork.EducationRepository.Get(request.updateEducationDto.Id);

        if (education != null)
        {
            _mapper.Map(request.updateEducationDto, education);

            var institutionLogoFile = await _photoAccessor.AddPhoto(request.updateEducationDto.EducationInstitutionLogoFile);

            if (institutionLogoFile != null){
                
                education.EducationInstitutionLogo = new Photo
                {
                    Id = institutionLogoFile.PublicId,
                    Url = institutionLogoFile.Url,
                    EducationInstitutionLogoId = education.Id
                };
            }

            await _unitOfWork.EducationRepository.Update(education);

            if (await _unitOfWork.Save() > 0)
                {
                response.IsSuccess = true;
                response.Error = "Education Updated Successfully.";
                response.Value = Unit.Value;
                }
                
            else{
                response.IsSuccess = false;
                response.Error = "Education Update Failed.";
                response.Value = null;
                }
        return response;
        }
        else{
            response.IsSuccess = false;
            response.Error = "Education Not Found.";
            response.Value = null;
            return response;

                
        }
    }
}
