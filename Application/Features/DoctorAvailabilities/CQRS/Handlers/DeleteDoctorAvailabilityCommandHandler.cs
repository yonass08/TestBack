using Application.Contracts.Persistence;
using Application.Features.DoctorAvailabilities.CQRS.Commands;
using Application.Responses;
using MediatR;

namespace Application.Features.DoctorAvailabilities.CQRS.Handlers
{
    public class DeleteDoctorAvailabilityCommandHandler : IRequestHandler<DeleteDoctorAvailabilityCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDoctorAvailabilityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeleteDoctorAvailabilityCommand request, CancellationToken cancellationToken)
        {

            var doctorAvailability = await _unitOfWork.DoctorAvailabilityRepository.Get(request.Id);

            if (doctorAvailability is null) return null;

            await _unitOfWork.DoctorAvailabilityRepository.Delete(doctorAvailability);

            if (await _unitOfWork.Save() > 0)
                return Result<Guid>.Success(doctorAvailability.Id);

            return Result<Guid>.Failure("Delete Failed");

        }
    }
}
