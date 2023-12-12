using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.Specialities.CQRS.Queries;
using Application.Features.Specialities.DTOs;
using MediatR;
using Application.Responses;

namespace Application.Features.Specialities.CQRS.Handlers
{
    public class GetSpecialityDetailQueryHandler : IRequestHandler<GetSpecialityDetailQuery, Result<SpecialityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSpecialityDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<SpecialityDto>> Handle(GetSpecialityDetailQuery request, CancellationToken cancellationToken)
        {
            var Speciality = await _unitOfWork.SpecialityRepository.Get(request.Id);

            if (Speciality == null) return null;

            return Result<SpecialityDto>.Success(_mapper.Map<SpecialityDto>(Speciality));
        }
    }
}