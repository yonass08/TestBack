using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.DoctorProfiles.CQRS.Queris;
using Application.Features.DoctorProfiles.DTOs;
using Application.Responses;
using MediatR;
using AutoMapper;
using Application.Features.InstitutionProfiles.DTOs;


namespace Application.Features.DoctorProfiles.CQRS.Handlers
{
    public class FilterDoctorProfilesQueryHandler : IRequestHandler<FilterDoctorProfilesQuery, Result<List<DoctorProfileDto>>>

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public FilterDoctorProfilesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<DoctorProfileDto>>> Handle(FilterDoctorProfilesQuery request, CancellationToken cancellationToken)
        {
            var response = new Result<List<DoctorProfileDto>>();
            var doctorProfiles = await _unitOfWork.DoctorProfileRepository.FilterDoctors(request.InstitutionId, request.Name, request.SpecialityNames, request.ExperienceYears, request.EducationName,request.pageNumber,request.pageSize);
            if (doctorProfiles is null)
            {
                response.IsSuccess = false;
                response.Error = "not found";
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
