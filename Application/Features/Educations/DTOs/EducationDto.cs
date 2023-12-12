using Application.Features.Common;
using Domain;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Educations.DTOs;

public class EducationDto : BaseDto, IEducationDto
{
    public string EducationInstitution { get; set; }
    public DateTime StartYear { get; set; }
    public DateTime GraduationYear { get; set; }
    public string Degree { get; set; }
    public string FieldOfStudy { get; set; }
    public string? EducationInstitutionLogoUrl { get; set; }
}
