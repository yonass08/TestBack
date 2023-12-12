using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.DoctorAvailabilities.CQRS.Queries;
using Application.Features.DoctorAvailabilities.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.DoctorAvailabilities.CQRS.Handlers
{
    public class GetDoctorAvailabilityListQueryHandler : IRequestHandler<GetDoctorAvailabilityListQuery, Result<List<DoctorAvailabilityDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDoctorAvailabilityListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<DoctorAvailabilityDto>>> Handle(GetDoctorAvailabilityListQuery request, CancellationToken cancellationToken)
        {
            var doctorAvailability = await _unitOfWork.DoctorAvailabilityRepository.GetAll();

            if (doctorAvailability == null) return null;

            return Result<List<DoctorAvailabilityDto>>.Success(_mapper.Map<List<DoctorAvailabilityDto>>(doctorAvailability));
        }
    }
}
