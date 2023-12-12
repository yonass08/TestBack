using Application.Contracts.Persistence;
using Application.Features.Services.CQRS.Queries;
using Application.Features.Services.DTOs;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.Services.CQRS.Handlers;
    public class GetServiceByInstitutionQueryHandler : IRequestHandler<GetServiceByInstitutionQuery, Result<List<ServiceDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetServiceByInstitutionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ServiceDto>>> Handle(GetServiceByInstitutionQuery request, CancellationToken cancellationToken)
        {
            var services = await _unitOfWork.ServiceRepository.GetServicesByInstitutionId(request.InstitutionId);

            if (services == null)
            {
                 return null;
            }

            var serviceDTOs = _mapper.Map<List<ServiceDto>>(services);

            return Result<List<ServiceDto>>.Success(serviceDTOs);

        }
    }

