using Application.Contracts.Persistence;
using Application.Features.Experiences.CQRS.Commands;
using Application.Responses;
using MediatR;

namespace Application.Features.Experiences.CQRS.Handlers;

public class DeleteExperienceCommandHandler : IRequestHandler<DeleteExperienceCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteExperienceCommandHandler(IUnitOfWork unitOfWork, AutoMapper.IMapper? _mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeleteExperienceCommand request, CancellationToken cancellationToken)
        {

            var experience = await _unitOfWork.ExperienceRepository.Get(request.Id);

            if (experience is null) return null;

            await _unitOfWork.ExperienceRepository.Delete(experience);

            if (await _unitOfWork.Save() > 0)
                return Result<Guid>.Success(experience.Id);

            return Result<Guid>.Failure("Delete Failed");

        }
    }


