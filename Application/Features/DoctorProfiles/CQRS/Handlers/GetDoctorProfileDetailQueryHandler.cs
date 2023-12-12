using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Features.DoctorProfiles.CQRS.Queris;
using Application.Features.DoctorProfiles.DTOs;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.DoctorProfiles.CQRS.Handlers
{
    public class GetDoctorProfileDetailQueryHandler : IRequestHandler<GetDoctorProfileDetialQuery, Result<DoctorProfileDetailDto>>
    {

        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        public GetDoctorProfileDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<DoctorProfileDetailDto>> Handle(GetDoctorProfileDetialQuery request, CancellationToken cancellationToken)
        {
            var response = new Result<DoctorProfileDetailDto>();
            var doctorProfile = await _unitOfWork.DoctorProfileRepository.GetDoctorProfileDetail(request.Id);
            if (doctorProfile is null)
            {
                var error = new NotFoundException(nameof(doctorProfile), request.Id).Message;
                response.IsSuccess = false;
                response.Error = $"{error}";
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Value = _mapper.Map<DoctorProfileDetailDto>(doctorProfile);
                return response;
            }




        }
    }
}