using FluentValidation;

namespace Application.Features.Specialities.DTOs.Validators

{
    public class UpdateSpecialityDtoValidator : AbstractValidator<UpdateSpecialityDto>
    {
        public UpdateSpecialityDtoValidator()
        {
            Include(new ISpecialityDtoValidator());

            RuleFor(p => p.Id).NotNull().WithMessage("{PropertyName} must be present");

        }
    }
}