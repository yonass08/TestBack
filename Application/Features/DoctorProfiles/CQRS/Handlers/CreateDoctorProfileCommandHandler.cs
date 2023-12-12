using Application.Contracts.Persistence;
using Application.Features.DoctorProfiles.CQRS.Commands;
using Application.Features.DoctorProfiles.DTOs.Validators;
using Application.Interfaces;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.DoctorProfiles.CQRS.Handlers
{
    public class CreateDoctorProfileCommandHandler : IRequestHandler<CreateDoctorProfileCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoAccessor _photoAccessor;

        public CreateDoctorProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<Guid>> Handle(CreateDoctorProfileCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateDoctorProfileDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateDoctorProfileDto);
            if (!validationResult.IsValid)
            {
                return Result<Guid>.Failure(string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var doctorProfile = _mapper.Map<DoctorProfile>(request.CreateDoctorProfileDto);

            if (request.CreateDoctorProfileDto.DoctorPhoto != null)
            {
                var photoUploadResult = await _photoAccessor.AddPhoto(request.CreateDoctorProfileDto.DoctorPhoto);
                if (photoUploadResult == null)
                {
                    return Result<Guid>.Failure("photo upload failed");
                }

                doctorProfile.Photo = new Photo
                {
                    Id = photoUploadResult.PublicId,
                    Url = photoUploadResult.Url
                };
            }

            await _unitOfWork.DoctorProfileRepository.Add(doctorProfile);
            if (await _unitOfWork.Save() == 0)
            {
                return Result<Guid>.Failure("Server error");
            }

            return Result<Guid>.Success(doctorProfile.Id);
        }
    }
}
