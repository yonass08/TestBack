using Domain;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Educations.DTOs;

public interface IEducationDto
{
    public Guid Id { get; set; }
    public string EducationInstitution { get; set; }
    public DateTime StartYear { get; set; }
    public DateTime GraduationYear { get; set; }
    public string Degree { get; set; }
    public string FieldOfStudy { get; set; }
    
}
