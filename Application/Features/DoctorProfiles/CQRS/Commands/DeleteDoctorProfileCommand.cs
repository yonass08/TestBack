using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Responses;
using MediatR;

namespace Application.Features.DoctorProfiles.CQRS.Commands
{
    public class DeleteDoctorProfileCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }

    }
}