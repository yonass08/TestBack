using API.Controllers;
using Application.Features.Specialities.CQRS.Commands;
using Application.Features.Specialities.CQRS.Queries;
using Application.Features.Specialities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SpecialitiesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public SpecialitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<SpecialityDto>>> Get()
        {
            return HandleResult(await _mediator.Send(new GetSpecialityListQuery()));
        }

        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateSpecialityDto createTask)
        {

            var command = new CreateSpecialityCommand { SpecialityDto = createTask };
            return HandleResult(await _mediator.Send(command));
        }
      

        [HttpPatch("{id}")]
        public async Task<IActionResult> Put([FromBody] UpdateSpecialityDto specialityDto, Guid id)
        {
            specialityDto.Id = id;
            var command = new UpdateSpecialityCommand { SpecialityDto = specialityDto };
            return HandleResult(await _mediator.Send(command));
        }

    
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteSpecialityCommand { Id = id };
            return HandleResult(await _mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResult(await _mediator.Send(new GetSpecialityDetailQuery { Id = id }));
        }

    }
}