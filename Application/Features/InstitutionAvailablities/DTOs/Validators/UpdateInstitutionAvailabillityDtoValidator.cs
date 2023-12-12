using FluentValidation;

namespace Application.Features.InstitutionAvailabilities.DTOs.Validators

{
    public class  UpdateInstitutionAvailabillityDtoValidator : AbstractValidator<UpdateInstitutionAvailabilityDto>
    {
        public UpdateInstitutionAvailabillityDtoValidator()
        {
            Include(new IInstitutionAvailabilityDtoValidator());

            RuleFor(p => p.Id).NotNull().WithMessage("{PropertyName} must be present");

        }
    }
}