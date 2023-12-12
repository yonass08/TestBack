using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.InstitutionProfiles.CQRS.Queries;
using Application.Responses;
using MediatR;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.InstitutionProfiles.CQRS.Handlers
{
    public class InstitutionProfileSearchQueryHandler : IRequestHandler<InstitutionProfileSearchQuery, Result<List<InstitutionProfileDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InstitutionProfileSearchQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<InstitutionProfileDto>>> Handle(InstitutionProfileSearchQuery request, CancellationToken cancellationToken)
        {
            var InstitutionProfiles = await _unitOfWork.InstitutionProfileRepository.Search(request.ServiceNames, request.OperationYears, request.OpenStatus, request.Name,request.pageNumber,request.pageSize,request.latitude,request.longitude,request.maxDistance);

            if (InstitutionProfiles == null) return null;

            return Result<List<InstitutionProfileDto>>.Success(_mapper.Map<List<InstitutionProfileDto>>(InstitutionProfiles));
        }
    }
}
