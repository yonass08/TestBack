using System.Data;
using Application.Features.Educations.CQRS;
using Application.Features.Educations.CQRS.Queries;
using Application.Features.Educations.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class EducationsController : BaseApiController
{
    private readonly IMediator _mediator;

    public EducationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<EducationDto>>> Get()
    {
        return HandleResult(await _mediator.Send(new GetEducationListQuery()));
    }

    [AllowAnonymous]
    [HttpGet("GetEducationInstitutionNameAndLogo")]
    public async Task<ActionResult<List<GetEducationInstitutionNameAndLogoDto>>> GetEducationInstitutionNameAndLogo()
    {
        return HandleResult(await _mediator.Send(new GetEducationInstitutionNameAndLogoQuery()));
    }
    
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return HandleResult(await _mediator.Send(new GetEducationDetailQuery { Id = id }));
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] CreateEducationDto createEducationDto)
    {

        var command = new CreateEducationCommand { createEducationDto = createEducationDto };
        return HandleResult(await _mediator.Send(command));
    }

   
    [HttpPatch("{id}")]
    public async Task<IActionResult> Put([FromForm] UpdateEducationDto updateEducationDto, Guid id)
    {
        updateEducationDto.Id = id;
        
        var command = new UpdateEducationCommand { updateEducationDto =  updateEducationDto};
        return HandleResult(await _mediator.Send(command));
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteEducationCommand { Id = id };
        return HandleResult(await _mediator.Send(command));
    }

}
