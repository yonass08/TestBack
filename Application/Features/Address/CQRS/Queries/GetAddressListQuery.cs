
using MediatR;
using Application.Responses;
using Application.Features.Addresses.DTOs;

namespace Application.Features.Addresses.CQRS.Queries
{
    public class GetAddressListQuery : IRequest<Result<List<AddressDto>>>

    {

    }
}