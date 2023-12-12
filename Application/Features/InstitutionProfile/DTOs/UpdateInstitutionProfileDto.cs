using Application.Features.Common;
using Domain;
using Microsoft.AspNetCore.Http;

namespace Application.Features.InstitutionProfiles.DTOs
{
    public class UpdateInstitutionProfileDto : BaseDto, IInstitutionProfileDto
    {
        public string InstitutionName { get; set; }
        public string BranchName { get; set; }
        public string Website { get; set; }
        public string PhoneNumber {get; set;}
        public string Summary {get; set;}
        public DateTime EstablishedOn {get; set;}
        public double Rate {get; set;}

        public IFormFile? LogoFile {get;set;}
        public IFormFile? BannerFile { get; set; }
        public ICollection<IFormFile>? PhotoFiles { get; set; }
    }
}