using Application.Contracts.Persistence;
using Application.Features.Services.CQRS.Commands;
using Application.Features.Services.DTOs.Validators;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.Services.CQRS.Handlers;

public class UpdateServiceCommandHandler  : IRequestHandler<UpdateServiceCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateServiceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateServiceDtoValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(request.ServiceDto);

            if (!validationResult.IsValid)
                return Result<Unit>.Failure(validationResult.Errors[0].ErrorMessage);


            var service = await _unitOfWork.ServiceRepository.Get(request.ServiceDto.Id);
            if (service == null) return null;

            _mapper.Map(request.ServiceDto, service);
            await _unitOfWork.ServiceRepository.Update(service);

            if (await _unitOfWork.Save() > 0)
                return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Update Failed");

        }
    }


