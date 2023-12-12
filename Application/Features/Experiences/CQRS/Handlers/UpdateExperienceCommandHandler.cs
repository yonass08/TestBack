using Application.Contracts.Persistence;
using Application.Features.Experiences.CQRS.Commands;
using Application.Features.Experiences.DTOs.Validators;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.Experiences.CQRS.Handlers;

public class UpdateExperienceCommandHandler  : IRequestHandler<UpdateExperienceCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateExperienceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(UpdateExperienceCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateExperienceDtoValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(request.ExperienceDto);

            if (!validationResult.IsValid)
                return Result<Unit>.Failure(validationResult.Errors[0].ErrorMessage);


            var experience = await _unitOfWork.ExperienceRepository.Get(request.ExperienceDto.Id);
            if (experience == null) return null;

            _mapper.Map(request.ExperienceDto, experience);
            await _unitOfWork.ExperienceRepository.Update(experience);

            if (await _unitOfWork.Save() > 0)
                return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Update Failed");

        }
    }


