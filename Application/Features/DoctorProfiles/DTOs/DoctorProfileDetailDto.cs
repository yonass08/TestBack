using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Common;
using Application.Features.Educations.DTOs;
using Application.Features.Experiences.DTOs;
using Domain;
using static Domain.DoctorProfile;

namespace Application.Features.DoctorProfiles.DTOs
{
    public class DoctorProfileDetailDto : BaseDto
    {
        public string FullName { get; set; }
        public string? About { get; set; }
        public string Gender { get; set; }
        public string? Email { get; set; }
        public string? PhotoUrl { get; set; }
        public int YearsOfExperience { get; set; }
        public Guid? MainInstitutionId { get; set; }
        public string? MainInstitutionName { get; set; }

        public ICollection<string> Specialities { get; set; } = new List<string>();

        public ICollection<EducationDto> Educations { get; set; } = new List<EducationDto>();
        public ICollection<ExperienceDto> Experiences { get; set; } = new List<ExperienceDto>();


    }
}