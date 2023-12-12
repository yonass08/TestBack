using System.Text.Json.Serialization;
using Domain.Common;
namespace Domain;



public class DoctorProfile : BaseDomainEntity
{
     public enum GenderType
        {
            Male,
            Female
        }

        public string FullName { get; set; }
        public string? About { get; set; }
        public GenderType Gender { get; set; }
        public string? Email { get; set; }
        public string? PhotoId { get; set; }
        //
        public Photo? Photo { get; set; }
        public DateTime CareerStartTime { get; set; }
        public Guid? MainInstitutionId { get; set; }
        //
        public InstitutionProfile? MainInstitution { get; set; }
        //
        public ICollection<InstitutionProfile> Institutions { get; set; } = new List<InstitutionProfile>();
        //
        public ICollection<DoctorAvailability> DoctorAvailabilities { get; set; } = new List<DoctorAvailability>();
        //
        public ICollection<Education> Educations { get; set; } = new List<Education>();
        //
        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
        //
        public ICollection<Speciality> Specialities { get; set; } = new List<Speciality>();
}
