using FluentValidation;

namespace Application.Features.DoctorAvailabilities.DTOs.Validators

{
    public class  UpdateDoctorAvailabillityDtoValidator : AbstractValidator<UpdateDoctorAvailabilityDto>
    {
        public UpdateDoctorAvailabillityDtoValidator()
        {
            Include(new IDoctorAvailabilityDtoValidator());

            RuleFor(p => p.Id).NotNull().WithMessage("{PropertyName} must be present");

        }
    }
}