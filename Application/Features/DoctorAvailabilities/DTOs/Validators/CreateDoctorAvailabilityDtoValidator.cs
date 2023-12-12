using FluentValidation;

namespace Application.Features.DoctorAvailabilities.DTOs.Validators
{
    public class CreateDoctorAvailabilityDtoValidator : AbstractValidator<CreateDoctorAvailabilityDto>
    {
        public CreateDoctorAvailabilityDtoValidator()
        {
            Include(new IDoctorAvailabilityDtoValidator());
        }
    }
}