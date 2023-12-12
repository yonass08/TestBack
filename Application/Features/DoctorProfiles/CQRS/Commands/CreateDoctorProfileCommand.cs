using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.DoctorProfiles.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.DoctorProfiles.CQRS.Commands
{
    public class CreateDoctorProfileCommand : IRequest<Result<Guid>>
    {
        public CreateDoctorProfileDto CreateDoctorProfileDto { get; set; }
    }
}