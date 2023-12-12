using FluentValidation;

namespace Application.Features.Educations.DTOs.Validators;

public class UpdateEducationDtoValidator : AbstractValidator<UpdateEducationDto>
{
    public UpdateEducationDtoValidator()
    {
        Include(new IEducationDtoValidator());
        RuleFor(p => p.FieldOfStudy).NotNull().WithMessage("{PropertyName} must be present");
       

    }
}
