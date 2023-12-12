using Application.Contracts.Persistence;
using FluentValidation;

namespace Application.Features.Experiences.DTOs.Validators;

public class CreateExperienceDtoValidator : AbstractValidator<CreateExperienceDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateExperienceDtoValidator(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
            Include(new IExperienceDtoValidator(_unitOfWork));
        }
    }
