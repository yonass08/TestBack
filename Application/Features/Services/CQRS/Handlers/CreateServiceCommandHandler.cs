using Application.Contracts.Persistence;
using Application.Features.Services.CQRS.Commands;
using Application.Features.Services.DTOs.Validators;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.Services.CQRS.Handlers;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

    public CreateServiceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<Result<Guid>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {

            var validator = new CreateServiceDtoValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(request.ServiceDto);

            if (!validationResult.IsValid)
                return Result<Guid>.Failure(validationResult.Errors[0].ErrorMessage);


            var service = _mapper.Map<Service>(request.ServiceDto);
            await _unitOfWork.ServiceRepository.Add(service);

            if (await _unitOfWork.Save() > 0)
                return Result<Guid>.Success(service.Id);

            return Result<Guid>.Failure("Creation Failed");

        }

   
}

