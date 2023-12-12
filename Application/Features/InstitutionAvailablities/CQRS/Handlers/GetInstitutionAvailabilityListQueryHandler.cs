using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.InstitutionAvailabilities.CQRS.Queries;
using Application.Features.InstitutionAvailabilities.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.InstitutionAvailabilities.CQRS.Handlers
{
    public class GetInstitutionAvailabilityListQueryHandler : IRequestHandler<GetInstitutionAvailabilityListQuery, Result<List<InstitutionAvailabilityDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInstitutionAvailabilityListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<InstitutionAvailabilityDto>>> Handle(GetInstitutionAvailabilityListQuery request, CancellationToken cancellationToken)
        {
            var institutionAvailability = await _unitOfWork.InstitutionAvailabilityRepository.GetAll();

            if (institutionAvailability == null) return null;

            return Result<List<InstitutionAvailabilityDto>>.Success(_mapper.Map<List<InstitutionAvailabilityDto>>(institutionAvailability));
        }
    }
}
