using Application.Contracts.Persistence;
using FluentValidation;

namespace Application.Features.Services.DTOs.Validators;

public class CreateServiceDtoValidator : AbstractValidator<CreateServiceDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateServiceDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Include(new IServiceDtoValidator(_unitOfWork));
        }
    }
