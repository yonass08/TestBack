using Domain.Common;
namespace Domain;
public class Education : BaseDomainEntity
{
    public string EducationInstitution { get; set; }
    public DateTime StartYear { get; set; }
    public DateTime GraduationYear { get; set; }
    public string Degree { get; set; }
    public string FieldOfStudy { get; set; }
    public Guid DoctorId { get; set; }
    public DoctorProfile Doctor { get; set; }
    public string? EducationInstitutionLogoId { get; set; }
    public Photo? EducationInstitutionLogo { get; set; }
}
