using Application.Features.InstitutionProfiles.DTOs;
using FluentValidation;
using System;

namespace Application.Features.InstitutionProfiles.DTOs.Validators
{
    public class IInstitutionProfileDtoValidator : AbstractValidator<IInstitutionProfileDto>
    {
        public IInstitutionProfileDtoValidator()
        {
            RuleFor(p => p.InstitutionName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed {MaxLength} characters.");

            RuleFor(p => p.BranchName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed {MaxLength} characters.");

            RuleFor(p => p.Website)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed {MaxLength} characters.")
                .Matches(@"^(https?:\/\/)?(www\.)?([a-zA-Z0-9-]{1,63}\.)+[a-zA-Z]{2,63}$")
                .WithMessage("{PropertyName} is not a valid URL.");

            RuleFor(p => p.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Summary)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(500).WithMessage("{PropertyName} must not exceed {MaxLength} characters.");

            RuleFor(p => p.EstablishedOn)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("{PropertyName} must be a past or present date.");

            RuleFor(p => p.Rate)
                .InclusiveBetween(0, 10).WithMessage("{PropertyName} must be a value between {From} and {To}.");
        }
    }
}
