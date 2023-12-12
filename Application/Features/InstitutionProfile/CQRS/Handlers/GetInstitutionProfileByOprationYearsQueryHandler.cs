using AutoMapper;
using Application.Contracts.Persistence;
using MediatR;
using Application.Responses;
using Application.Features.InstitutionProfiles.CQRS.Queries;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.InstitutionProfiles.CQRS.Handlers
{
    public class GetInstitutionProfileByOprationYearsQueryHandler : IRequestHandler<GetInstitutionProfileByOprationYearsQuery, Result<List<InstitutionProfileDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInstitutionProfileByOprationYearsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<InstitutionProfileDto>>> Handle(GetInstitutionProfileByOprationYearsQuery request, CancellationToken cancellationToken)
        {
            var InstitutionProfile = await _unitOfWork.InstitutionProfileRepository.GetByYears(request.Years);
            if (InstitutionProfile == null) return Result<List<InstitutionProfileDto>>.Failure(error: "Item not found.");

            return Result<List<InstitutionProfileDto>>.Success(_mapper.Map<List<InstitutionProfileDto>>(InstitutionProfile));
        }
    }
}