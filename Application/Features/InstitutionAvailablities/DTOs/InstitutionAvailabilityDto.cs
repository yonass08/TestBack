using Application.Features.Common;

namespace Application.Features.InstitutionAvailabilities.DTOs
{
    public class InstitutionAvailabilityDto : BaseDto, IInstitutionAvailabilityDto
    {
        public string StartDay { get; set; }
        public string EndDay { get; set; }
        public string Opening { get; set; }
        public string Closing { get; set; }
        public bool TwentyFourHours { get; set; }
    }
}