using AutoMapper;
using Application.Contracts.Persistence;
using Application.Responses;
using MediatR;
using Application.Features.Addresses.CQRS.Queries;
using Application.Features.Addresses.DTOs;

namespace Application.Features.Addresses.CQRS.Handlers
{
    public class GetAddressListQueryHandler : IRequestHandler<GetAddressListQuery, Result<List<AddressDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAddressListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<AddressDto>>> Handle(GetAddressListQuery request, CancellationToken cancellationToken)
        {
            var Addresses = await _unitOfWork.AddressRepository.GetAllPopulated();

            if (Addresses == null) return Result<List<AddressDto>>.Failure("No Address Found.");


            return Result<List<AddressDto>>.Success(_mapper.Map<List<AddressDto>>(Addresses));
        }
    }
}
