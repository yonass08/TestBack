using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Common;
using Domain;
using Microsoft.AspNetCore.Http;
using static Domain.DoctorProfile;

namespace Application.Features.DoctorProfiles.DTOs
{
    public class UpdateDoctorProfileDto : BaseDto
    {
        public string FullName { get; set; }
        public string About { get; set; }
        public string Email { get; set; }
        public IFormFile DoctorPhoto { get; set; }
        public DateTime CareerStartTime { get; set; }
        public string Gender { get; set; }
        



    }
}