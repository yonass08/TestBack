using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.DoctorAvailabilities.CQRS.Queries;
using Application.Features.DoctorAvailabilities.DTOs;
using MediatR;
using Application.Responses;

namespace Application.Features.DoctorAvailabilities.CQRS.Handlers
{
    public class GetDoctorAvailabilityDetailQueryHandler : IRequestHandler<GetDoctorAvailabilityDetailQuery, Result<DoctorAvailabilityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDoctorAvailabilityDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<DoctorAvailabilityDto>> Handle(GetDoctorAvailabilityDetailQuery request, CancellationToken cancellationToken)
        {
            var doctorAvailability = await _unitOfWork.DoctorAvailabilityRepository.Get(request.Id);

            if (doctorAvailability == null) return null;

            return Result<DoctorAvailabilityDto>.Success(_mapper.Map<DoctorAvailabilityDto>(doctorAvailability));
        }
    }
}