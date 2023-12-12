using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.Specialities.CQRS.Queries;
using Application.Features.Specialities.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Specialities.CQRS.Handlers
{
    public class GetSpecialityListQueryHandler : IRequestHandler<GetSpecialityListQuery, Result<List<SpecialityDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSpecialityListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<SpecialityDto>>> Handle(GetSpecialityListQuery request, CancellationToken cancellationToken)
        {
            var Specialities = await _unitOfWork.SpecialityRepository.GetAll();

            if (Specialities == null) return null;

            return Result<List<SpecialityDto>>.Success(_mapper.Map<List<SpecialityDto>>(Specialities));
        }
    }
}
