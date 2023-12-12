
using MediatR;
using Application.Responses;
using Application.Features.Addresses.DTOs;

namespace Application.Features.Addresses.CQRS.Queries
{
    public class GetAddressDetailQuery : IRequest<Result<AddressDto>>
    {
        public Guid Id { get; set; }
    }
}