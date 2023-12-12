using Application.Contracts.Persistence;
using Application.Features.Experiences.CQRS.Commands;
using Application.Features.Experiences.DTOs.Validators;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.Experiences.CQRS.Handlers;

public class CreateExperienceCommandHandler : IRequestHandler<CreateExperienceCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CreateExperienceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<Result<Guid>> Handle(CreateExperienceCommand request, CancellationToken cancellationToken)
        {

            var validator = new CreateExperienceDtoValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(request.ExperienceDto);

            if (!validationResult.IsValid)
                return Result<Guid>.Failure(validationResult.Errors[0].ErrorMessage);


            var experience = _mapper.Map<Experience>(request.ExperienceDto);
            await _unitOfWork.ExperienceRepository.Add(experience);

            if (await _unitOfWork.Save() > 0)
                return Result<Guid>.Success(experience.Id);

            return Result<Guid>.Failure("Creation Failed");

        }
    }

