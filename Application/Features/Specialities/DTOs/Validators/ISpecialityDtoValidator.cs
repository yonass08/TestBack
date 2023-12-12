using FluentValidation;

namespace Application.Features.Specialities.DTOs.Validators
{
    public class ISpecialityDtoValidator : AbstractValidator<ISpecialityDto>
    {
        public ISpecialityDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(300).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");



        }

    }
}