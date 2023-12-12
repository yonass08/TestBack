using FluentValidation;

namespace Application.Features.InstitutionAvailabilities.DTOs.Validators
{
    public class CreateInstitutionAvailabilityDtoValidator : AbstractValidator<CreateInstitutionAvailabilityDto>
    {
        public CreateInstitutionAvailabilityDtoValidator()
        {
            Include(new IInstitutionAvailabilityDtoValidator());
        }
    }
}