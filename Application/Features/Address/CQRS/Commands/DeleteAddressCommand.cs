using MediatR;
using Application.Responses;

namespace Application.Features.Addresses.CQRS.Commands
{
    public class DeleteAddressCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }
}
