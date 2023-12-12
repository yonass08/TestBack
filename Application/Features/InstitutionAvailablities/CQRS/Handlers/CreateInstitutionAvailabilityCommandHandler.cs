
using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.InstitutionAvailabilities.CQRS.Commands;
using Application.Features.InstitutionAvailabilities.DTOs.Validators;
using Application.Responses;
using MediatR;
using Domain;

namespace Application.Features.InstitutionAvailabilities.CQRS.Handlers
{
    public class CreateInstitutionAvailabilityCommandHandler : IRequestHandler<CreateInstitutionAvailabilityCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CreateInstitutionAvailabilityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<Result<Guid>> Handle(CreateInstitutionAvailabilityCommand request, CancellationToken cancellationToken)
        {

            var validator = new CreateInstitutionAvailabilityDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateInstitutionAvailabilityDto);

            if (!validationResult.IsValid)
                return Result<Guid>.Failure(validationResult.Errors[0].ErrorMessage);


            var institutionAvailability = _mapper.Map<InstitutionAvailability>(request.CreateInstitutionAvailabilityDto);
            await _unitOfWork.InstitutionAvailabilityRepository.Add(institutionAvailability);

            if (await _unitOfWork.Save() > 0)
                return Result<Guid>.Success(institutionAvailability.Id);
                // return Result<Unit>.Success(Unit.Value);

            return Result<Guid>.Failure("Creation Failed");

        }
    }
}
