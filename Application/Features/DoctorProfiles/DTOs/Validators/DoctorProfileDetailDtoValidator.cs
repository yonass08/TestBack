using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;


namespace Application.Features.DoctorProfiles.DTOs.Validators
{

    public class DoctorProfileDetailDtoValidator : AbstractValidator<DoctorProfileDetailDto>
    {
        public DoctorProfileDetailDtoValidator()
        {

            RuleFor(dto => dto.FullName)
            .NotEmpty()
            .WithMessage("FullName is required");
            RuleFor(dto => dto.About)
            .MaximumLength(1000)
            .WithMessage("About cannot exceed 1000 characters");

            RuleFor(dto => dto.Gender)
            .IsInEnum().WithMessage("Invalid Gender value");

            RuleFor(dto => dto.Email)
            .NotEmpty().EmailAddress()
            .WithMessage("Email is required and should be a valid email address");

            // RuleFor(dto => dto.PhotoId)
            // .NotEmpty().WithMessage("PhotoId is required");

            // RuleFor(dto => dto.CareerStartTime)
            // .NotEmpty().WithMessage("CareerStartTime is required");
            // RuleFor(dto => dto.MainInstitutionId)
            // .NotEmpty().WithMessage("MainInstitutionId is required");


        }
    }

}