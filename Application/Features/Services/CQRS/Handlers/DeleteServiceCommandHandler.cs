using Application.Contracts.Persistence;
using Application.Features.Services.CQRS.Commands;
using Application.Responses;
using MediatR;

namespace Application.Features.Services.CQRS.Handlers;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteServiceCommandHandler(IUnitOfWork unitOfWork, AutoMapper.IMapper _mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {

            var service = await _unitOfWork.ServiceRepository.Get(request.Id);

            if (service is null) return null;

            await _unitOfWork.ServiceRepository.Delete(service);

            if (await _unitOfWork.Save() > 0)
                return Result<Guid>.Success(service.Id);

            return Result<Guid>.Failure("Delete Failed");

        }
    }

