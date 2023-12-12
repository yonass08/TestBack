using FluentValidation;

namespace Application.Features.DoctorAvailabilities.DTOs.Validators
{
    public class IDoctorAvailabilityDtoValidator : AbstractValidator<IDoctorAvailabilityDto>
    {
        public IDoctorAvailabilityDtoValidator()
        {
            RuleFor(p => p.DoctorId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.EndTime)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
                
            RuleFor(p => p.StartTime)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();


        }

    }
}