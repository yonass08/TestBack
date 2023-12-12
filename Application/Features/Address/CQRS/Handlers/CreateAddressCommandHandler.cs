
using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.Specialities.DTOs.Validators;
using Application.Responses;
using MediatR;
using Domain;
using Application.Features.Addresses.CQRS.Commands;
using Application.Features.Addresses.DTOs.Validators;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.Addresses.CQRS.Handlers
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CreateAddressCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<Result<Guid>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {

            var validator = new CreateAddressDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateAddressDto);

            if (!validationResult.IsValid)
                return Result<Guid>.Failure(validationResult.Errors[0].ErrorMessage);

            InstitutionProfile institution = await _unitOfWork.InstitutionProfileRepository.GetPopulatedInstitution(request.CreateAddressDto.InstitutionId);

            if(institution != null)
            {
                if (institution.Address != null)
                {
                    return Result<Guid>.Failure(institution.InstitutionName+" has exiting address.");
                }

                var Address = _mapper.Map<Address>(request.CreateAddressDto);
                await _unitOfWork.AddressRepository.Add(Address);

                if (await _unitOfWork.Save() > 0)
                    return Result<Guid>.Success(Address.Id);
            }
               
            return Result<Guid>.Failure("Creation Failed");

        }
    }
}
