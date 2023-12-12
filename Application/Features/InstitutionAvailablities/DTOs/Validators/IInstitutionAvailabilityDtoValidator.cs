using FluentValidation;

namespace Application.Features.InstitutionAvailabilities.DTOs.Validators
{
    public class IInstitutionAvailabilityDtoValidator : AbstractValidator<IInstitutionAvailabilityDto>
    {
        public IInstitutionAvailabilityDtoValidator()
        {                
            RuleFor(dto => dto.StartDay).NotEmpty().WithMessage("Start day is required.");
            
            RuleFor(dto => dto.EndDay).NotEmpty().WithMessage("End day is required.");
            
            RuleFor(dto => dto.Opening)
            .NotEmpty().WithMessage("Opening time is required.")
            .Matches(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$").WithMessage("Invalid opening time format.");

            RuleFor(dto => dto.Closing)
                .NotEmpty().WithMessage("Closing time is required.")
                .Matches(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$").WithMessage("Invalid closing time format.");

            RuleFor(dto => dto.TwentyFourHours)
                .NotNull().WithMessage("TwentyFourHours property must be specified.");

        }

    }
}