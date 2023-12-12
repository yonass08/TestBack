using FluentValidation;
using Application.Contracts.Persistence;

namespace Application.Features.Experiences.DTOs.Validators;

public class UpdateExperienceDtoValidator : AbstractValidator<UpdateExperienceDto>
    {
         private readonly IUnitOfWork _unitOfWork;

        public UpdateExperienceDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Include(new IExperienceDtoValidator(_unitOfWork));
            
            RuleFor(dto => dto.Id).NotNull().WithMessage("{PropertyName} must be present");

        }
    }
