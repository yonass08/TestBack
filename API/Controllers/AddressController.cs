using API.Controllers;
using Application.Features.Addresses.CQRS.Commands;
using Application.Features.Addresses.CQRS.Queries;
using Application.Features.Addresses.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AddressesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AddressesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<AddressDto>>> Get()
        {
            return HandleResult(await _mediator.Send(new GetAddressListQuery()));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAddressDto createTask)
        {

            var command = new CreateAddressCommand { CreateAddressDto = createTask };
            return HandleResult(await _mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] UpdateAddressDto AddressDto, Guid id)
        {
            AddressDto.Id = id;
            var command = new UpdateAddressCommand { UpdateAddressDto = AddressDto };
            return HandleResult(await _mediator.Send(command));
        }
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteAddressCommand { Id = id };
            return HandleResult(await _mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResult(await _mediator.Send(new GetAddressDetailQuery { Id = id }));
        }

    }
}