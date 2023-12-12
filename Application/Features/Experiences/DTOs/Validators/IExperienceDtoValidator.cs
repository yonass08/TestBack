using FluentValidation;
using Application.Contracts.Persistence;

namespace Application.Features.Experiences.DTOs.Validators;

public class IExperienceDtoValidator : AbstractValidator<IExperienceDto>
    {

        private readonly IUnitOfWork _unitOfWork;

        public IExperienceDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(dto => dto.Position)
                .NotEmpty().WithMessage("Position is required.")
                .MaximumLength(100).WithMessage("Position must not exceed 100 characters.");

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(dto => dto.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .LessThanOrEqualTo(dto => dto.EndDate).WithMessage("Start date must be before or equal to end date.");

            RuleFor(dto => dto.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(dto => dto.StartDate).WithMessage("End date must be after or equal to start date.");

            
            RuleFor(x => x.InstitutionId)
                .NotEmpty().WithMessage("Institution ID is required.");

                // RuleFor(p => p.DoctorId)
                //                 .NotEmpty().WithMessage("Doctor ID is required.");

                // .MustAsync(async (doctorId, token) =>
                // {
                //     var existingUser = await _unitOfWork.

                //     return existingUser == null;
                // })
                // .WithMessage("{PropertyName} does not exist!");
        }
    }