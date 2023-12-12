using FluentValidation;

namespace Application.Features.Specialities.DTOs.Validators
{
    public class CreateSpecialityDtoValidator : AbstractValidator<CreateSpecialityDto>
    {
        public CreateSpecialityDtoValidator()
        {
            Include(new ISpecialityDtoValidator());
        }
    }
}