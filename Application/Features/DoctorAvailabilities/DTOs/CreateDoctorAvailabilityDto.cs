namespace Application.Features.DoctorAvailabilities.DTOs
{
    public class CreateDoctorAvailabilityDto : IDoctorAvailabilityDto
    {
        public DayOfWeek Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Guid DoctorId { get; set; }

        public Guid InstitutionId { get; set; }

        public Guid SpecialityId { get; set; }
    }
}