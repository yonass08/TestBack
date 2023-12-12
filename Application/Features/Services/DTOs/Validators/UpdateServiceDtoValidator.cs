using Application.Contracts.Persistence;
using FluentValidation;

namespace Application.Features.Services.DTOs.Validators;

public class UpdateServiceDtoValidator : AbstractValidator<UpdateServiceDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateServiceDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Include(new IServiceDtoValidator(_unitOfWork));
            
            RuleFor(dto => dto.Id).NotNull().WithMessage("{PropertyName} must be present");

        }
    }
