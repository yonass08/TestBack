using Application.Features.Experiences.CQRS.Commands;
using Application.Features.Experiences.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class ExperiencesController : BaseApiController
{
    private readonly IMediator _mediator;

    public ExperiencesController(IMediator mediator)
    {
        _mediator = mediator;
    }
 

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateExperienceDto createExperienceDto)
    {
        var command = new CreateExperienceCommand { ExperienceDto = createExperienceDto };
        return HandleResult(await _mediator.Send(command));
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromBody] UpdateExperienceDto experienceDto, Guid id)
    {
        experienceDto.Id = id;
        var command = new UpdateExperienceCommand { ExperienceDto = experienceDto };
        return HandleResult(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteExperienceCommand { Id = id };
        return HandleResult(await _mediator.Send(command));
    }
    

}
