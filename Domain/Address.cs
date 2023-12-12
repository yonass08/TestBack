using Domain.Common;

namespace Domain
{
    public class Address : BaseDomainEntity
    {
        public string Country { get; set; }
        public string? Region { get; set; }
        public string? Zone { get; set; }
        public string? Woreda { get; set; }
        public string City { get; set; }
        public string? SubCity { get; set; }

        public string? Summary { get; set; }

        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public Guid InstitutionId {get; set;}

        // One to one relation reference
        public InstitutionProfile Institution {get; set;}
    }
}