using API.Controllers;
using Application.Features.InstitutionProfiles.CQRS.Commands;
using Application.Features.InstitutionProfiles.CQRS.Queries;
using Application.Features.InstitutionProfiles.DTOs;
using Application.Features.Specialities.CQRS.Queries;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class InsitutionProfileController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IPhotoAccessor _photoAccessor;

        public InsitutionProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<InstitutionProfileDto>>> Get()
        {
            return HandleResult(await _mediator.Send(new GetInstitutionProfileListQuery()));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResult(await _mediator.Send(new GetInstitutionProfileDetailQuery { Id = id }));
        }

        [AllowAnonymous]
        [HttpGet("by-years/{years}")]
        public async Task<IActionResult> GetByYears(int years)
        {
            return HandleResult(await _mediator.Send(new GetInstitutionProfileByOprationYearsQuery { Years = years }));
        }

        [AllowAnonymous]
        [HttpGet("by-service/{serviceId}")]
        public async Task<IActionResult> GetByService(Guid serviceId)
        {
            return HandleResult(await _mediator.Send(new GetInstitutionProfileByServiceQuery { ServiceId = serviceId }));
        }

        [AllowAnonymous]
        [HttpPost("search-institutions")]
        public async Task<IActionResult> Search(ICollection<string>? serviceName = null, int operationYears = -1, bool openStatus = false, string? name = "",int pageNumber=0, int PageSize=0,double? latitude = null,double? longitude=null,double? maxDistance=null)
        {
            return HandleResult(await _mediator.Send(new InstitutionProfileSearchQuery { ServiceNames = serviceName, OperationYears = operationYears, OpenStatus = openStatus, Name = name,pageNumber=pageNumber,pageSize=PageSize ,latitude=latitude,longitude=longitude,maxDistance=maxDistance}));
        }

        [AllowAnonymous]
        [HttpGet("search-by-name")]
        public async Task<IActionResult> SearchByName(string Name)
        {
            return HandleResult(await _mediator.Send(new InstitutionProfileSearchByNameQuery { Name = Name }));
        }

        [AllowAnonymous]

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateInstitutionProfileDto createTask)
        {
            var command = new CreateInstitutionProfileCommand { CreateInstitutionProfileDto = createTask };
            return HandleResult(await _mediator.Send(command));
        }

        [AllowAnonymous]

        [HttpPatch("{id}")]
        public async Task<IActionResult> Put([FromForm] UpdateInstitutionProfileDto InstitutionProfileDto, Guid id)
        {
            InstitutionProfileDto.Id = id;
            var command = new UpdateInstitutionProfileCommand { UpdateInstitutionProfileDto = InstitutionProfileDto };
            return HandleResult(await _mediator.Send(command));
        }

        [AllowAnonymous]
  
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteInstitutionProfileCommand { Id = id };
            return HandleResult(await _mediator.Send(command));
        }
    }
}