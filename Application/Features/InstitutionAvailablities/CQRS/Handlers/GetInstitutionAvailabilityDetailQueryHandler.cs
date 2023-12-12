using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.InstitutionAvailabilities.CQRS.Queries;
using Application.Features.InstitutionAvailabilities.DTOs;
using MediatR;
using Application.Responses;

namespace Application.Features.InstitutionAvailabilities.CQRS.Handlers
{
    public class GetInstitutionAvailabilityDetailQueryHandler : IRequestHandler<GetInstitutionAvailabilityDetailQuery, Result<InstitutionAvailabilityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInstitutionAvailabilityDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<InstitutionAvailabilityDto>> Handle(GetInstitutionAvailabilityDetailQuery request, CancellationToken cancellationToken)
        {
            var institutionAvailability = await _unitOfWork.InstitutionAvailabilityRepository.Get(request.Id);

            if (institutionAvailability == null) return null;

            return Result<InstitutionAvailabilityDto>.Success(_mapper.Map<InstitutionAvailabilityDto>(institutionAvailability));
        }
    }
}