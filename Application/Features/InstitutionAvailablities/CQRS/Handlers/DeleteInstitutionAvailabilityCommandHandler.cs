using Application.Contracts.Persistence;
using Application.Features.InstitutionAvailabilities.CQRS.Commands;
using Application.Responses;
using MediatR;

namespace Application.Features.InstitutionAvailabilities.CQRS.Handlers
{
    public class DeleteInstitutionAvailabilityCommandHandler : IRequestHandler<DeleteInstitutionAvailabilityCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInstitutionAvailabilityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeleteInstitutionAvailabilityCommand request, CancellationToken cancellationToken)
        {

            var institutionAvailability = await _unitOfWork.InstitutionAvailabilityRepository.Get(request.Id);

            if (institutionAvailability is null) return null;

            await _unitOfWork.InstitutionAvailabilityRepository.Delete(institutionAvailability);

            if (await _unitOfWork.Save() > 0)
                return Result<Guid>.Success(institutionAvailability.Id);

            return Result<Guid>.Failure("Delete Failed");

        }
    }
}
