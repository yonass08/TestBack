using MediatR;
using Application.Responses;
using Application.Features.Addresses.DTOs;

namespace Application.Features.Addresses.CQRS.Commands
{
    public class CreateAddressCommand : IRequest<Result<Guid>>
    {
        public CreateAddressDto CreateAddressDto { get; set; }
    }
}
