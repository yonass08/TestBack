namespace Domain;
public class Photo
{
    public string Id { get; set; }
    public string Url { get; set; }
    public Guid? InstitutionProfileId { get; set; }
    public Guid? DoctorProfileId { get; set; }
    public Guid? LogoId { get; set; }
    public Guid? BannerId { get; set; }
    public Guid? EducationInstitutionLogoId { get; set; }
}
