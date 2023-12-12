using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Features.DoctorProfiles.CQRS.Commands;
using Application.Features.DoctorProfiles.DTOs.Validators;
using Application.Interfaces;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.DoctorProfiles.CQRS.Handlers
{
    public class UpdateDoctorProfileCommandHandler : IRequestHandler<UpdateDoctorProfileCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoAccessor _photoAccessor;

        public UpdateDoctorProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<Unit>> Handle(UpdateDoctorProfileCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateDoctorProfileDtoValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(request.updateDoctorProfileDto);

            if (!validationResult.IsValid)
            {
                return Result<Unit>.Failure(string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var doctorProfile = await _unitOfWork.DoctorProfileRepository.GetDoctorProfileDetail(request.updateDoctorProfileDto.Id);
            if (doctorProfile == null)
            {
                return Result<Unit>.Failure(new NotFoundException(nameof(doctorProfile), request.updateDoctorProfileDto.Id).Message);
            }


            _mapper.Map(request.updateDoctorProfileDto, doctorProfile);

            if (request.updateDoctorProfileDto.DoctorPhoto != null)
            {
                var photoUploadResult = await _photoAccessor.AddPhoto(request.updateDoctorProfileDto.DoctorPhoto);
                if (photoUploadResult == null)
                {
                    return Result<Unit>.Failure("Photo upload failed");
                }

                doctorProfile.Photo = new Photo
                {
                    Id = photoUploadResult.PublicId,
                    Url = photoUploadResult.Url
                };
            }

            await _unitOfWork.DoctorProfileRepository.Update(doctorProfile);

            if (await _unitOfWork.Save() == 0)
            {
                return Result<Unit>.Failure("Server error");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }

}