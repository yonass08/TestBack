
using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.DoctorAvailabilities.CQRS.Commands;
using Application.Features.DoctorAvailabilities.DTOs.Validators;
using Application.Responses;
using MediatR;
using Domain;

namespace Application.Features.DoctorAvailabilities.CQRS.Handlers
{
    public class CreateDoctorAvailabilityCommandHandler : IRequestHandler<CreateDoctorAvailabilityCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CreateDoctorAvailabilityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<Result<Guid>> Handle(CreateDoctorAvailabilityCommand request, CancellationToken cancellationToken)
        {

            var validator = new CreateDoctorAvailabilityDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateDoctorAvailabilityDto);

            if (!validationResult.IsValid)
                return Result<Guid>.Failure(validationResult.Errors[0].ErrorMessage);


            var doctorAvailability = _mapper.Map<DoctorAvailability>(request.CreateDoctorAvailabilityDto);
            await _unitOfWork.DoctorAvailabilityRepository.Add(doctorAvailability);

            if (await _unitOfWork.Save() > 0)
                return Result<Guid>.Success(doctorAvailability.Id);

            return Result<Guid>.Failure("Creation Failed");

        }
    }
}
