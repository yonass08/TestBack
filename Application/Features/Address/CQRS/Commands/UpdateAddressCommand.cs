
using MediatR;
using Application.Responses;
using Application.Features.Addresses.DTOs;

namespace Application.Features.Addresses.CQRS.Commands
{
    public class UpdateAddressCommand : IRequest<Result<Unit>>
    {
        public UpdateAddressDto UpdateAddressDto { get; set; }

    }
}
