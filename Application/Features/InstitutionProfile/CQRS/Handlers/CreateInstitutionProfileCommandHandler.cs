
using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.Specialities.DTOs.Validators;
using Application.Responses;
using MediatR;
using Domain;
using Newtonsoft.Json;
using Application.Features.InstitutionProfiles.CQRS.Commands;
using Application.Features.InstitutionProfiles.DTOs.Validators;
using Application.Interfaces;
using Application.Photos;
using Application.Features.Addresses.DTOs;

namespace Application.Features.InstitutionProfiles.CQRS.Handlers
{
    public class CreateInstitutionProfileCommandHandler : IRequestHandler<CreateInstitutionProfileCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoAccessor _photoAccessor;


        public CreateInstitutionProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;

        }

        public async Task<Result<Guid>> Handle(CreateInstitutionProfileCommand request, CancellationToken cancellationToken)
        {

            var validator = new CreateInstitutionProfileDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateInstitutionProfileDto);

            if (!validationResult.IsValid){
                
                return Result<Guid>.Failure(validationResult.Errors[0].ErrorMessage);
            }
            var uploadedLogo = await _photoAccessor.AddPhoto(request.CreateInstitutionProfileDto.LogoFile);
            var uploadedBanner = await _photoAccessor.AddPhoto(request.CreateInstitutionProfileDto.BannerFile);
            if (uploadedBanner == null || uploadedLogo == null) return Result<Guid>.Failure("Unable to upload logo/banner");
            var InstitutionProfile = _mapper.Map<InstitutionProfile>(request.CreateInstitutionProfileDto);

            Guid logoId = Guid.NewGuid();
            Photo logo = new Photo { Id = logoId.ToString(), Url = uploadedLogo.Url };
            Guid bannerId = Guid.NewGuid();
            Photo banner = new Photo { Id = bannerId.ToString(), Url = uploadedBanner.Url };

            InstitutionProfile.Logo = logo;
            InstitutionProfile.Banner = banner;
            InstitutionProfile.BannerId = uploadedBanner.PublicId;
            InstitutionProfile.LogoId = uploadedLogo.PublicId;
            InstitutionProfile institutionProfile = await _unitOfWork.InstitutionProfileRepository.Add(InstitutionProfile);

            
            if (await _unitOfWork.Save() > 0 )
                return Result<Guid>.Success(InstitutionProfile.Id);
                
            return Result<Guid>.Failure("Creation Failed");

        }
    }
}
