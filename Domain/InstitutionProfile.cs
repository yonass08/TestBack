

using System.Text.Json.Serialization;
using Domain.Common;
namespace Domain
{
    public class InstitutionProfile : BaseDomainEntity
    {
        public string InstitutionName { get; set; }
        public string? BranchName { get; set; }
        public string? Website { get; set; }
        public string? PhoneNumber { get; set; }
        public string Summary { get; set; }
        public DateTime EstablishedOn { get; set; }
        public double Rate { get; set; }
        //
        public Address? Address { get; set; }
        public string LogoId { get; set; }
        //
        public Photo Logo { get; set; }
        public string BannerId { get; set; }
        //
        public Photo Banner { get; set; }
        //
        public ICollection<DoctorProfile> Doctors { get; set; } = new List<DoctorProfile>();

        //
        public ICollection<Service> Services { get; set; } = new List<Service>();
        //
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
        //
        public InstitutionAvailability InstitutionAvailability { get; set; }

    }
}