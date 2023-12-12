using Application.Contracts.Persistence;
using Application.Features.Services.CQRS.Queries;
using Application.Features.Services.DTOs;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.Services.CQRS.Handlers;

    public class GetServiceByNameQueryHandler : IRequestHandler<GetServiceByNameQuery, Result<ServiceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;        
        public GetServiceByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ServiceDto>> Handle(GetServiceByNameQuery request, CancellationToken cancellationToken)
        {
            var service = await _unitOfWork.ServiceRepository.GetServiceByName(request.ServiceName);

            if (service == null)
            {
                return null;
            }

            var serviceDTO = _mapper.Map<ServiceDto>(service);

            return Result<ServiceDto>.Success(serviceDTO);
        }
    }

