using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using static Domain.DoctorProfile;

namespace Application.Features.DoctorProfiles.DTOs
{
    public class CreateDoctorProfileDto
    {
        public string FullName { get; set; }
        public string? About { get; set; }
        public string Gender { get; set; }
        public IFormFile DoctorPhoto { get; set; }
        public string? Email { get; set; }
        public DateTime CareerStartTime { get; set; }


    }
}