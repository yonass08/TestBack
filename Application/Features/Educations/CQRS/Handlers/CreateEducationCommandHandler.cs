
using System.Net;
using AutoMapper;
using Application.Contracts.Persistence;
using Application.Responses;
using MediatR;
using Domain;
using Application.Features.Educations.DTOs;
using Application.Features.Educations.DTOs.Validators;
using Application.Interfaces;

namespace Application.Features.Educations.CQRS.Handlers;

public class CreateEducationCommandHandler: IRequestHandler<CreateEducationCommand, Result<CreateEducationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPhotoAccessor _photoAccessor;

    public CreateEducationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _photoAccessor = photoAccessor;
    }

    public async Task<Result<CreateEducationDto>> Handle(CreateEducationCommand request, CancellationToken cancellationToken)
    {

        var validator = new CreateEducationDtoValidators();
        var validationResult = await validator.ValidateAsync(request.createEducationDto);

        if (!validationResult.IsValid)
            return Result<CreateEducationDto>.Failure(validationResult.Errors[0].ErrorMessage);
        var response = new Result<CreateEducationDto>();
        
        // create an Id for the education model
        request.createEducationDto.Id = Guid.NewGuid();

        // parsing the photo
        var institutionLogoFile = await _photoAccessor.AddPhoto(request.createEducationDto.EducationInstitutionLogoFile);

        // if adding the photo fails return photo parsing failed response
        if (institutionLogoFile != null){
           
            // request.createEducationDto.EducationInstitutionLogoId = institutionLogoFile.PublicId;
            var education = _mapper.Map<Education>(request.createEducationDto);
            education.EducationInstitutionLogo  = new Photo
            {
                Id = institutionLogoFile.PublicId,
                Url = institutionLogoFile.Url,
                EducationInstitutionLogoId = request.createEducationDto.Id
            };

            var edu = await _unitOfWork.EducationRepository.Add(education);

            if (await _unitOfWork.Save() > 0){
                response.Value = _mapper.Map<CreateEducationDto>(edu);
                response.IsSuccess = true;
                response.Error = "Create Education Successful.";
                
            }else{
                response.Value = null;
                response.IsSuccess = false;
                response.Error = "Create Education Failed.";
            }
                return response;
            }
        else{
            response.Value = null;
            response.IsSuccess = false;
            response.Error = "Photo Not Found.";
            return response;
        }
    }
}
