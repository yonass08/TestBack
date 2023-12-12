using Application.Features.Services.CQRS.Commands;
using Application.Features.Services.CQRS.Queries;
using Application.Features.Services.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class ServicesController : BaseApiController
{
    private readonly IMediator _mediator;

    public ServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }
 

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateServiceDto createServiceDto)
    {
        var command = new CreateServiceCommand { ServiceDto = createServiceDto };
        return HandleResult(await _mediator.Send(command));
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromBody] UpdateServiceDto serviceDto, Guid id)
    {
        serviceDto.Id = id;
        var command = new UpdateServiceCommand { ServiceDto = serviceDto };
        return HandleResult(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteServiceCommand { Id = id };
        return HandleResult(await _mediator.Send(command));
    }
            
    [AllowAnonymous]
    [HttpGet("institution/{institutionId}")]
    public async Task<IActionResult> GetServiceByInstitution(Guid institutionId)
    {
        var query = new GetServiceByInstitutionQuery { InstitutionId = institutionId };
        return HandleResult(await _mediator.Send(query));

    }

    [AllowAnonymous]
    [HttpGet("{serviceName}")]
    public async Task<IActionResult> GetServiceByName(string serviceName)
    {
        var query = new GetServiceByNameQuery { ServiceName = serviceName };
        return HandleResult(await _mediator.Send(query));
    }

}
