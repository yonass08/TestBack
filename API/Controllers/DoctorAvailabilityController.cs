using API.Controllers;
using Application.Features.DoctorAvailabilities.CQRS.Commands;
using Application.Features.DoctorAvailabilities.CQRS.Queries;
using Application.Features.DoctorAvailabilities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class DoctorAvailabilityController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DoctorAvailabilityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<DoctorAvailabilityDto>>> Get()
        {
            return HandleResult(await _mediator.Send(new GetDoctorAvailabilityListQuery()));
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateDoctorAvailabilityDto createTask)
        {

            var command = new CreateDoctorAvailabilityCommand { CreateDoctorAvailabilityDto = createTask };
            return HandleResult(await _mediator.Send(command));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] UpdateDoctorAvailabilityDto doctorAvailabilityDto, Guid id)
        {
            doctorAvailabilityDto.Id = id;
            var command = new UpdateDoctorAvailabilityCommand { UpdateDoctorAvailabilityDto = doctorAvailabilityDto };
            return HandleResult(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteDoctorAvailabilityCommand { Id = id };
            return HandleResult(await _mediator.Send(command));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResult(await _mediator.Send(new GetDoctorAvailabilityDetailQuery { Id = id }));
        }

    }
}