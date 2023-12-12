using Application.Features.InstitutionProfiles.DTOs.Validators;
using Application.Features.InstitutionProfiles.DTOs;
using FluentValidation;

namespace Application.Features.InstitutionProfiles.DTOs.Validators

{
    public class UpdateInstitutionProfileDtoValidator : AbstractValidator<UpdateInstitutionProfileDto>
    {
        public UpdateInstitutionProfileDtoValidator()
        {
            RuleFor(p => p.InstitutionName)
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed {MaxLength} characters.")
                .When(p => p.InstitutionName != null);

            RuleFor(p => p.BranchName)
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed {MaxLength} characters.")
                .When(p => p.BranchName != null);

            RuleFor(p => p.Website)
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed {MaxLength} characters.")
                .Matches(@"^(https?:\/\/)?(www\.)?([a-zA-Z0-9-]{1,63}\.)+[a-zA-Z]{2,63}$")
                .WithMessage("{PropertyName} is not a valid URL.")
                .When(p => p.Website != null);

            RuleFor(p => p.Summary)
                .MaximumLength(500).WithMessage("{PropertyName} must not exceed {MaxLength} characters.")
                .When(p => p.Summary != null);

            RuleFor(p => p.EstablishedOn)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("{PropertyName} must be a past or present date.")
                .When(p => p.EstablishedOn != null);

            RuleFor(p => p.Rate)
                .InclusiveBetween(0, 10).WithMessage("{PropertyName} must be a value between {From} and {To}.")
                .When(p => p.Rate != null);

        }
    }
}