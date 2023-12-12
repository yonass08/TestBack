using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.DoctorProfiles.CQRS.Commands;
using Application.Features.DoctorProfiles.CQRS.Queris;
using Application.Features.DoctorProfiles.DTOs;
using Application.Features.InstitutionProfiles.DTOs;
using Application.Responses;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Domain.DoctorProfile;

namespace API.Controllers
{
    [Route("api/[Controller]")]
    public class DoctorProfilesController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<DoctorProfileDetailDto>>> GetDoctorProfileDetail(Guid id)
        {
            var query = new GetDoctorProfileDetialQuery { Id = id };
            var response = await Mediator.Send(query);
            return HandleResult<DoctorProfileDetailDto>(response);


        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<Result<List<DoctorProfileDto>>>> GetAllDoctors(){
            var query = new GetDoctorProfileListQuery();
            var response = await Mediator.Send(query);
            return HandleResult<List<DoctorProfileDto>>(response);
        } 

        [AllowAnonymous]
        [HttpPost("filter")]
        public async Task<ActionResult<List<DoctorProfileDto>>> FilterDoctors(string? name, ICollection<string>? specialityNames = null, string? educationName = "", int experienceYears = -1, Guid institutionId = new Guid(),int pageNumber=0,int pageSize=0)
        {
            var query = new FilterDoctorProfilesQuery
            {
                SpecialityNames = specialityNames,
                EducationName = educationName,
                ExperienceYears = experienceYears,
                InstitutionId = institutionId,
                pageNumber = pageNumber,
                pageSize = pageSize,
                Name = name
            };
            var response = await Mediator.Send(query);
            return HandleResult<List<DoctorProfileDto>>(response);
        }
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult<Guid>> Post([FromForm] CreateDoctorProfileDto createDoctorProfileDto)
        {
            var command = new CreateDoctorProfileCommand { CreateDoctorProfileDto = createDoctorProfileDto };
            var response = await Mediator.Send(command);
            return HandleResult<Guid>(response);
        }
        [AllowAnonymous]
        [HttpPatch("update")]
        public async Task<ActionResult<Unit>> Update([FromForm] UpdateDoctorProfileDto updateDoctorProfileDto)
        {
            var command = new UpdateDoctorProfileCommand { updateDoctorProfileDto = updateDoctorProfileDto };
            var response = await Mediator.Send(command);
            return HandleResult<Unit>(response);
        }
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {

            var command = new DeleteDoctorProfileCommand { Id = id };
            var response = await Mediator.Send(command);
            return HandleResult<Unit>(response);

        }




    }
}