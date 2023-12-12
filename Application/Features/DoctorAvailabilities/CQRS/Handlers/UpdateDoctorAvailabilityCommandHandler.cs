using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.DoctorAvailabilities.CQRS.Commands;
using Application.Features.DoctorAvailabilities.DTOs.Validators;
using MediatR;
using Application.Responses;

namespace Application.Features.DoctorAvailabilities.CQRS.Handlers
{
    public class UpdateDoctorAvailabilityCommandHandler : IRequestHandler<UpdateDoctorAvailabilityCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDoctorAvailabilityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(UpdateDoctorAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateDoctorAvailabillityDtoValidator();
            var validationResult = await validator.ValidateAsync(request.UpdateDoctorAvailabilityDto);

            if (!validationResult.IsValid)
                return Result<Unit>.Failure(validationResult.Errors[0].ErrorMessage);


            var doctorAvailability = await _unitOfWork.DoctorAvailabilityRepository.Get(request.UpdateDoctorAvailabilityDto.Id);
            if (doctorAvailability == null) return null;

            _mapper.Map(request.UpdateDoctorAvailabilityDto, doctorAvailability);
            await _unitOfWork.DoctorAvailabilityRepository.Update(doctorAvailability);

            if (await _unitOfWork.Save() > 0)
                return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Update Failed");

        }
    }
}
