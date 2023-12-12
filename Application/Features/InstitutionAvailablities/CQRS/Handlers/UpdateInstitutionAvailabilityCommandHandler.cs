using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.InstitutionAvailabilities.CQRS.Commands;
using Application.Features.InstitutionAvailabilities.DTOs.Validators;
using MediatR;
using Application.Responses;

namespace Application.Features.InstitutionAvailabilities.CQRS.Handlers
{
    public class UpdateInstitutionAvailabilityCommandHandler : IRequestHandler<UpdateInstitutionAvailabilityCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateInstitutionAvailabilityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(UpdateInstitutionAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateInstitutionAvailabillityDtoValidator();
            var validationResult = await validator.ValidateAsync(request.UpdateInstitutionAvailabilityDto);

            if (!validationResult.IsValid)
                return Result<Unit>.Failure(validationResult.Errors[0].ErrorMessage);


            var institutionAvailability = await _unitOfWork.InstitutionAvailabilityRepository.Get(request.UpdateInstitutionAvailabilityDto.Id);
            if (institutionAvailability == null) return null;

            _mapper.Map(request.UpdateInstitutionAvailabilityDto, institutionAvailability);
            await _unitOfWork.InstitutionAvailabilityRepository.Update(institutionAvailability);

            if (await _unitOfWork.Save() > 0)
                return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Update Failed");

        }
    }
}
