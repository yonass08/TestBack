using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Application.Photos;
using Application.Features.Educations.DTOs;
using Application.Features.InstitutionAvailabilities.DTOs;
using Application.Features.InstitutionProfiles.DTOs;
using Application.Features.Addresses.DTOs;
using Application.Features.Experiences.DTOs;
using Application.Features.Services.DTOs;
using Application.Features.Specialities.DTOs;
using Application.Features.DoctorAvailabilities.DTOs;

using AutoMapper;
using Domain;
using Application.Features.DoctorProfiles.DTOs;

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateSpecialityDto, Speciality>().ReverseMap();
            CreateMap<UpdateSpecialityDto, Speciality>().ReverseMap();
            CreateMap<Speciality, SpecialityDto>().ReverseMap();

            CreateMap<CreateEducationDto, Education>().ReverseMap();
            CreateMap<UpdateEducationDto, Education>().ReverseMap();
            CreateMap<Education, EducationDto>()
            .ForMember(x => x.EducationInstitutionLogoUrl, o => o.MapFrom(s => s.EducationInstitutionLogo.Url))
            .ReverseMap();
            CreateMap<Education, EducationalInstitutionDto>()
            .ForMember(x => x.LogoUrl, o => o.MapFrom(s => s.EducationInstitutionLogo.Url))
            .ForMember(x => x.InstitutionName, o => o.MapFrom(s => s.EducationInstitution))
            .ReverseMap();
            CreateMap<Education, GetEducationInstitutionNameAndLogoDto>().ReverseMap();

            CreateMap<CreateDoctorAvailabilityDto, DoctorAvailability>().ReverseMap();
            CreateMap<UpdateDoctorAvailabilityDto, DoctorAvailability>().ReverseMap();
            CreateMap<DoctorAvailability, DoctorAvailabilityDto>();

            CreateMap<CreateInstitutionAvailabilityDto, InstitutionAvailability>().ReverseMap();
            CreateMap<UpdateInstitutionAvailabilityDto, InstitutionAvailability>().ReverseMap();
            CreateMap<InstitutionAvailability, InstitutionAvailabilityDto>()
            .ForMember(dest => dest.StartDay, opt => opt.MapFrom(src => src.StartDay.ToString()))
            .ForMember(dest => dest.EndDay, opt => opt.MapFrom(src => src.EndDay.ToString()))
            .ReverseMap();
            

            CreateMap<CreateInstitutionProfileDto, InstitutionProfile>().ReverseMap();
            CreateMap<UpdateInstitutionProfileDto, InstitutionProfile>().ReverseMap();
            CreateMap<InstitutionProfile, InstitutionProfileDto>()
            .ForMember(x => x.BannerUrl, o => o.MapFrom(s => s.Banner.Url))
            .ForMember(x => x.LogoUrl, o => o.MapFrom(s => s.Logo.Url))
             .ForMember(
                dest => dest.Services,
                opt => opt.MapFrom(src => src.Services.Select(service => service.ServiceName).ToList())
            )
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ComputeOpenCloseStatus(src.InstitutionAvailability)))
            .ReverseMap();

            CreateMap<InstitutionProfile, InstitutionProfileDetailDto>()
           .ForMember(x => x.BannerUrl, o => o.MapFrom(s => s.Banner.Url))
           .ForMember(x => x.LogoUrl, o => o.MapFrom(s => s.Logo.Url))
            .ForMember(
               dest => dest.Services,
               opt => opt.MapFrom(src => src.Services.Select(service => service.ServiceName).ToList())
           )
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ComputeOpenCloseStatus(src.InstitutionAvailability)))
            .ForMember(
               dest => dest.Photos,
               opt => opt.MapFrom(src => src.Photos.Select(photo => photo.Url).ToList())
           )
                       .ForMember(dest => dest.InstitutionAvailability, opt => opt.MapFrom(src => src.InstitutionAvailability))

           .ReverseMap();

            CreateMap<DoctorProfile, InstitutionDoctorDto>()
            .ForMember(x => x.PhotoUrl, o => o.MapFrom(s => s.Photo.Url))
            .ForMember(x => x.MainInstitutionName, o => o.MapFrom(s => s.MainInstitution.InstitutionName))

             .ForMember(
               dest => dest.Specialities,
               opt => opt.MapFrom(src => src.Specialities.Select(Speciality => Speciality.Name).ToList())
           )
               .ForMember(dest => dest.YearsOfExperience, opt => opt.MapFrom(src => CalculateYearsOfExperience(src.CareerStartTime)))

            .ReverseMap();


            CreateMap<CreateAddressDto, Address>().ReverseMap();
            CreateMap<UpdateAddressDto, Address>().ReverseMap();
            CreateMap<AddressDto, Address>().ReverseMap();

            CreateMap<CreateExperienceDto, Experience>().ReverseMap();
            CreateMap<UpdateExperienceDto, Experience>().ReverseMap();
            CreateMap<Experience, ExperienceDto>()
            .ForMember(x => x.InstitutionName, o => o.MapFrom(s => s.Institution.InstitutionName))
            .ReverseMap();

            CreateMap<CreateServiceDto, Service>().ReverseMap();
            CreateMap<UpdateServiceDto, Service>().ReverseMap();
            CreateMap<ServiceDto, Service>().ReverseMap();

            CreateMap<DoctorProfile, DoctorProfileDto>()
            .ForMember(x => x.PhotoUrl, o => o.MapFrom(s => s.Photo.Url))
            .ForMember(x => x.MainInstitutionName, o => o.MapFrom(s => s.MainInstitution.InstitutionName))
            .ForMember(dest => dest.YearsOfExperience, opt => opt.MapFrom(src => CalculateYearsOfExperience(src.CareerStartTime)))
            .ForMember(
               dest => dest.Specialities,
               opt => opt.MapFrom(src => src.Specialities.Select(Speciality => Speciality.Name).ToList())
           )
            .ReverseMap();
            CreateMap<DoctorProfile, DoctorProfileDetailDto>()
            .ForMember(x => x.PhotoUrl, o => o.MapFrom(s => s.Photo.Url))
            .ForMember(x => x.MainInstitutionName, o => o.MapFrom(s => s.MainInstitution.InstitutionName))

                         .ForMember(
               dest => dest.Specialities,
               opt => opt.MapFrom(src => src.Specialities.Select(Speciality => Speciality.Name).ToList())
           )
               .ForMember(dest => dest.YearsOfExperience, opt => opt.MapFrom(src => CalculateYearsOfExperience(src.CareerStartTime)))

            .ReverseMap();
            CreateMap<CreateDoctorProfileDto, DoctorProfile>();
            CreateMap<UpdateDoctorProfileDto, DoctorProfile>();



        }
        private static int CalculateYearsOfExperience(DateTime careerStartTime)
        {
            int years = DateTime.Now.Year - careerStartTime.Year;
            if (DateTime.Now.Month < careerStartTime.Month || (DateTime.Now.Month == careerStartTime.Month && DateTime.Now.Day < careerStartTime.Day))
            {
                years--;
            }

            Random random = new Random();
            int randomYears = random.Next(1, 6);

            return years + randomYears;
        }

        public string ComputeOpenCloseStatus(InstitutionAvailability availability)
        {
            // Get the current day of the week
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;

            // Check if the institution is open 24 hours
            if (availability.TwentyFourHours)
            {
                return "Open"; // Always open if 24 hours
            }
            
            // Check if the current day is within the availability range
            
            if (currentDay >= availability.StartDay && (availability.EndDay == DayOfWeek.Sunday || currentDay <= availability.EndDay))
            {
                
                // Get the current time
                TimeSpan currentTime = DateTime.Now.TimeOfDay;

                // Parse the opening and closing times
                TimeSpan openingTime = TimeSpan.Parse(availability.Opening);
                TimeSpan closingTime = TimeSpan.Parse(availability.Closing);

                // Check if the current time is within the opening and closing times
                if (currentTime >= openingTime && currentTime <= closingTime)
                {
                    return "Open"; // Open
                }
            }

            return "Close"; // Closed
        }


    }
}