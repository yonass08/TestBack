using Domain.Common;
namespace Domain;

public class Experience : BaseDomainEntity
{
    public string Position { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Guid DoctorId { get; set; }
    public DoctorProfile Doctor { get; set; }

    public Guid InstitutionId { get; set; }
    public InstitutionProfile Institution { get; set; }

}
