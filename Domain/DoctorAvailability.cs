using Domain.Common;

namespace Domain
{
    public class DoctorAvailability : BaseDomainEntity
    {
        public DayOfWeek Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Guid DoctorId { get; set; }
        public DoctorProfile Doctor { get; set; }

        public Guid InstitutionId { get; set; }
        public InstitutionProfile Institution { get; set; }

        public Guid SpecialityId { get; set; }
        public Speciality Speciality { get; set; }
    }
}