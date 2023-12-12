using Application.Contracts.Persistence;
using FluentValidation;

namespace Application.Features.Services.DTOs.Validators;
public class IServiceDtoValidator : AbstractValidator<IServiceDto>
{
    private readonly IUnitOfWork _unitOfWork;
    public IServiceDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;


        RuleFor(dto => dto.ServiceDescription)
            .NotEmpty().WithMessage("Service description is required.")
            .MaximumLength(500).WithMessage("Service description must not exceed 500 characters.");

        RuleFor(dto => dto.ServiceName)
                .NotEmpty().WithMessage("Service name is required.")
                .MaximumLength(100).WithMessage("Service name must not exceed 100 characters.")
                .MustAsync(async (serviceName, cancellationToken) =>
                {
                    var existingService = await _unitOfWork.ServiceRepository.GetServiceByName(serviceName);
                    return existingService == null;
                }).WithMessage("Service name already exists.");

    }
    
}
