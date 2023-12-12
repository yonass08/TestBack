using Domain.Common;
namespace Domain
{
    public class InstitutionAvailability : BaseDomainEntity
    {
        public DayOfWeek StartDay { get; set; }
        public DayOfWeek EndDay { get; set; }
        public string Opening { get; set; }
        public string Closing { get; set; }
        public bool TwentyFourHours { get; set; }
        public Guid InstitutionId { get; set; }
        public InstitutionProfile Institution { get; set; }
    }
}