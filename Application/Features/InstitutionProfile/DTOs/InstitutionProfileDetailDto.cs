using Application.Features.Common;
using Application.Features.InstitutionAvailabilities.DTOs;
using Domain;
using Microsoft.AspNetCore.Http;
using Application.Features.Addresses.DTOs;
using Application.Features.Services.DTOs;

namespace Application.Features.InstitutionProfiles.DTOs
{
    public class InstitutionProfileDetailDto : BaseDto, IInstitutionProfileDto
    {
        public string InstitutionName { get; set; }
        public string BranchName { get; set; }
        public string Website { get; set; }
        public string PhoneNumber { get; set; }
        public string Summary { get; set; }
        public DateTime EstablishedOn { get; set; }
        public double Rate { get; set; }

        public string Status { get; set; }
        public ICollection<EducationalInstitutionDto> AllEducationalInstitutions { get; set; }
        public ICollection<string> AllSpecialities { get; set; }

        public string LogoUrl { get; set; }
        public string BannerUrl { get; set; }

        public InstitutionAvailabilityDto InstitutionAvailability { get; set; }
        public AddressDto Address { get; set; }
        public ICollection<string> Services { get; set; }

        public ICollection<string> Photos { get; set; }

        public ICollection<InstitutionDoctorDto> Doctors { get; set; }


    }
}