using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.Specialities.CQRS.Queries;
using Application.Responses;
using MediatR;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.InstitutionProfiles.CQRS.Handlers
{
    public class InstitutionProfileSearchByNameQueryHandler : IRequestHandler<InstitutionProfileSearchByNameQuery, Result<List<InstitutionProfileDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InstitutionProfileSearchByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<InstitutionProfileDto>>> Handle(InstitutionProfileSearchByNameQuery request, CancellationToken cancellationToken)
        {
            var InstitutionProfiles = await _unitOfWork.InstitutionProfileRepository.Search(request.Name);

            if (InstitutionProfiles == null)             return Result<List<InstitutionProfileDto>>.Failure(error: "Item not found.");

            return Result<List<InstitutionProfileDto>>.Success(_mapper.Map<List<InstitutionProfileDto>>(InstitutionProfiles));
        }
    }
}
