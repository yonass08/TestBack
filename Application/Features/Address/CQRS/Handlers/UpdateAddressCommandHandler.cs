using AutoMapper;
using Application.Contracts.Persistence;
using MediatR;
using Application.Responses;
using Application.Features.Addresses.CQRS.Commands;
using Application.Features.Addresses.DTOs.Validators;

namespace Application.Features.Addresses.CQRS.Handlers
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAddressCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateAddressDtoValidator();
            var validationResult = await validator.ValidateAsync(request.UpdateAddressDto);

            if (!validationResult.IsValid)
                return Result<Unit>.Failure(validationResult.Errors[0].ErrorMessage);


            var Address = await _unitOfWork.AddressRepository.Get(request.UpdateAddressDto.Id);
            if (Address == null) return Result<Unit>.Failure("Update Failed");

            _mapper.Map(request.UpdateAddressDto, Address);
            await _unitOfWork.AddressRepository.Update(Address);

            if (await _unitOfWork.Save() > 0)
                return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Update Failed");

        }
    }
}
