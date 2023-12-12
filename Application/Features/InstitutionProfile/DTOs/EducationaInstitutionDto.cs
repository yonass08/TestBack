using Application.Features.Common;
using Domain;
using Microsoft.AspNetCore.Http;

namespace Application.Features.InstitutionProfiles.DTOs
{
    public class EducationalInstitutionDto
    {
        public string InstitutionName { get; set; }
        public string? LogoUrl { get; set; }
    
    }
}