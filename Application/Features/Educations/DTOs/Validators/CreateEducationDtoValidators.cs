using FluentValidation;

namespace Application.Features.Educations.DTOs.Validators;

public class CreateEducationDtoValidators : AbstractValidator<CreateEducationDto>
{
    public CreateEducationDtoValidators()
    {
        Include(new IEducationDtoValidator());
        RuleFor(p => p.EducationInstitution)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");

        RuleFor(p => p.FieldOfStudy)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull()
                    .MaximumLength(50).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");
        RuleFor(p => p.FieldOfStudy)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull()
                    .MaximumLength(50).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");

        RuleFor(p => p.FieldOfStudy)
                            .NotEmpty().WithMessage("{PropertyName} is required.")
                            .NotNull()
                            .MaximumLength(50).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");

    }

}
