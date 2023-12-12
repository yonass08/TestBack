namespace Application.Features.InstitutionAvailabilities.DTOs
{
    public class CreateInstitutionAvailabilityDto : IInstitutionAvailabilityDto
    {
       public string StartDay { get; set; }
        public string EndDay { get; set; }
        public string Opening { get; set; }
        public string Closing { get; set; }
        public bool TwentyFourHours { get; set; }
        public Guid InstitutionId { get; set; }
    }
}