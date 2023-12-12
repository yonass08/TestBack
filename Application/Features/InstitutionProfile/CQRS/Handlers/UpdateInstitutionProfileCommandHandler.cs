using AutoMapper;
using Application.Contracts.Persistence;
using MediatR;
using Application.Responses;
using Application.Features.InstitutionProfiles.CQRS.Commands;
using Application.Features.InstitutionProfiles.DTOs.Validators;
using Domain;
using Application.Interfaces;
using Newtonsoft.Json;

namespace Application.Features.InstitutionProfiles.CQRS.Handlers
{
    public class UpdateInstitutionProfileCommandHandler : IRequestHandler<UpdateInstitutionProfileCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoAccessor _photoAccessor;


        public UpdateInstitutionProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;

        }

        public async Task<Result<Unit>> Handle(UpdateInstitutionProfileCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateInstitutionProfileDtoValidator();
            var validationResult = await validator.ValidateAsync(request.UpdateInstitutionProfileDto);


            if (!validationResult.IsValid)
                return Result<Unit>.Failure(validationResult.Errors[0].ErrorMessage);
            var InstitutionProfile = await _unitOfWork.InstitutionProfileRepository.Get(request.UpdateInstitutionProfileDto.Id);
           

            if (InstitutionProfile == null) return Result<Unit>.Failure("Update Failed");
            _mapper.Map(request.UpdateInstitutionProfileDto, InstitutionProfile);
            if (request.UpdateInstitutionProfileDto.LogoFile != null)
            {
                var uploadedLogo = await _photoAccessor.AddPhoto(request.UpdateInstitutionProfileDto.LogoFile);
                Guid logoId = Guid.NewGuid();
                Photo logo = new Photo { Id = logoId.ToString(), Url = uploadedLogo.Url };
                InstitutionProfile.Logo = logo;
            }

            if (request.UpdateInstitutionProfileDto.BannerFile != null)
            {
                var uploadedBanner = await _photoAccessor.AddPhoto(request.UpdateInstitutionProfileDto.BannerFile);
                Guid bannerId = Guid.NewGuid();
                Photo banner = new Photo { Id = bannerId.ToString(), Url = uploadedBanner.Url };
                InstitutionProfile.Banner = banner;
            }



            if (request.UpdateInstitutionProfileDto.PhotoFiles != null && request.UpdateInstitutionProfileDto.PhotoFiles.Count > 0)
            {
                foreach (var file in request.UpdateInstitutionProfileDto.PhotoFiles)
                {
                    var uploadedFile = await _photoAccessor.AddPhoto(file);
                    Guid photoId = Guid.NewGuid();

                    Photo photo = new Photo { Id = photoId.ToString(), Url = uploadedFile.Url, InstitutionProfileId = InstitutionProfile.Id };
                    if (await _unitOfWork.Save() <= 0) return Result<Unit>.Failure("Update Failed");
                    InstitutionProfile.Photos.Add(photo);
                }
            }

            await _unitOfWork.InstitutionProfileRepository.Update(InstitutionProfile);
            if (await _unitOfWork.Save() > 0)
                return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Update Failed");

        }
    }
}
