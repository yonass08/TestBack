using Application.Features.InstitutionProfiles.DTOs;
using Application.Features.InstitutionProfiles.DTOs.Validators;
using FluentValidation;

namespace Application.Features.Specialities.DTOs.Validators
{
    public class CreateInstitutionProfileDtoValidator : AbstractValidator<CreateInstitutionProfileDto>
    {
        public CreateInstitutionProfileDtoValidator()
        {
            Include(new IInstitutionProfileDtoValidator());
        }
    }
}