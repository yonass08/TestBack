using Domain.Common;
namespace Domain
{
    public class Speciality : BaseDomainEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<DoctorProfile> Doctors { get; set; } =  new List<DoctorProfile>();
    }
}