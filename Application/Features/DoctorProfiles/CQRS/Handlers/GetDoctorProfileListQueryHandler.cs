using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.DoctorProfiles.CQRS.Queris;
using Application.Responses;
using Application.Features.DoctorProfiles.DTOs;
using MediatR;

namespace Application.Features.DoctorProfiles.CQRS.Handlers
{
    public class GetDoctorProfileListQueryHandler : IRequestHandler<GetDoctorProfileListQuery, Result<List<DoctorProfileDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetDoctorProfileListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<DoctorProfileDto>>> Handle(GetDoctorProfileListQuery request, CancellationToken cancellationToken)
        {
            var response = new Result<List<DoctorProfileDto>>();
            var doctorProfiles = await _unitOfWork.DoctorProfileRepository.GetAllDoctors();
            if (doctorProfiles is null)
            {
                response.IsSuccess = false;
                response.Error = "No doctor profile";
                return response;

            }
            else
            {
                response.IsSuccess = true;
                response.Value = _mapper.Map<List<DoctorProfileDto>>(doctorProfiles);
                return response;
            }
        }

    }
}
