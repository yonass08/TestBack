using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Domain;
using Moq;
using static Domain.DoctorProfile;

namespace Application.UnitTest.Mocks
{

    public class MockDoctorProfileRepository
    {
        public static Mock<IDoctorProfileRepository> GetDoctorProfileRepository()
        {
            var doctorProfiles = new List<DoctorProfile>
                {
                    // doctor 1
                    new DoctorProfile
                    {
                        Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3301"),
                        FullName = "Dr. John Smith",
                        About = "Experienced and skilled doctor",
                        Gender = DoctorProfile.GenderType.Male,
                        Email = "john.smith@example.com",
                        PhotoId = "photo1",
                        CareerStartTime = DateTime.Parse("2022-02-10T09:15:26.533993Z"),
                        MainInstitutionId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3302"),
                        Institutions = new List<InstitutionProfile>
                        {
                            new InstitutionProfile
                            {
                                Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3302"),
                                InstitutionName = "General Hospital",
                                BranchName = "Main Branch",
                                Website = "http://generalhospital.com",
                                PhoneNumber = "123-456-7890",
                                Summary = "A leading hospital providing excellent healthcare services.",
                                EstablishedOn = DateTime.Parse("1980-01-01"),
                                Rate = 4.5,                                LogoId = "logo1",
                                BannerId = "banner1"
                            }
                        },
                        DoctorAvailabilities = new List<DoctorAvailability>
                        {
                            new DoctorAvailability
                            {
                                Id = Guid.NewGuid(),
                                Day = DayOfWeek.Monday,
                                StartTime = "09:00 AM",
                                EndTime = "05:00 PM",
                                InstitutionId = Guid.NewGuid(),
                                SpecialityId = Guid.NewGuid()
                            }
                        },
                        Educations = new List<Education>
                        {
                            new Education
                            {
                                Id = Guid.NewGuid(),
                                EducationInstitution = "Medical University",
                                StartYear = DateTime.Parse("2005-01-01"),
                                GraduationYear = DateTime.Parse("2010-01-01"),
                                Degree = "Doctor of Medicine",
                                FieldOfStudy = "General Medicine"
                            }
                        },
                        Experiences = new List<Experience>
                        {
                            new Experience
                            {
                                Id = Guid.NewGuid(),
                                Position = "Senior Doctor",
                                StartDate = DateTime.Parse("2010-01-01"),
                                EndDate = DateTime.Parse("2020-12-31"),
                                InstitutionId = Guid.NewGuid()
                            }
                        },
                        Specialities = new List<Speciality>
                        {
                            new Speciality
                            {
                                Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3303"),
                                Name = "Internal Medicine",
                                Description = "Specializes in diagnosing and treating internal diseases."
                            }
                        }
                    },

                    // doctor 2
                    new DoctorProfile
                    {
                        Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3304"),
                        FullName = "Dr. Emily Johnson",
                        About = "Compassionate and dedicated doctor",
                        Gender = DoctorProfile.GenderType.Female,
                        Email = "emily.johnson@example.com",
                        PhotoId = "photo3",
                        CareerStartTime =DateTime.Parse("2022-06-10T09:15:26.533993Z"),
                        MainInstitutionId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3305"),
                        Institutions = new List<InstitutionProfile>
                        {
                            new InstitutionProfile
                            {
                                Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3305"),
                                InstitutionName = "City Hospital",
                                BranchName = "Downtown Branch",
                                Website = "http://cityhospital.com",
                                PhoneNumber = "987-654-3210",
                                Summary = "Providing quality healthcare services to the community.",
                                EstablishedOn = DateTime.Parse("1995-07-20"),
                                Rate = 4.2,
                                LogoId = "logo3",
                                BannerId = "banner3"
                            }
                        },
                        DoctorAvailabilities = new List<DoctorAvailability>
                        {
                            new DoctorAvailability
                            {
                                Id = Guid.NewGuid(),
                                Day = DayOfWeek.Tuesday,
                                StartTime = "10:00 AM",
                                EndTime = "06:00 PM",
                                InstitutionId = Guid.NewGuid(),
                                SpecialityId = Guid.NewGuid()
                            },
                            new DoctorAvailability
                            {
                                Id = Guid.NewGuid(),
                                Day = DayOfWeek.Wednesday,
                                StartTime = "08:00 AM",
                                EndTime = "04:00 PM",
                                InstitutionId = Guid.NewGuid(),
                                SpecialityId = Guid.NewGuid()
                            }
                        },
                        Educations = new List<Education>
                        {
                            new Education
                            {
                                Id = Guid.NewGuid(),
                                EducationInstitution = "Medical College",
                                StartYear = DateTime.Parse("2002-09-01"),
                                GraduationYear = DateTime.Parse("2008-06-30"),
                                Degree = "Doctor of Medicine",
                                FieldOfStudy = "General Medicine"
                            },
                            new Education
                            {
                                Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3303"),
                                EducationInstitution = "University of Specialization",
                                StartYear = DateTime.Parse("2008-07-01"),
                                GraduationYear = DateTime.Parse("2011-06-30"),
                                Degree = "Specialization in Cardiology",
                                FieldOfStudy = "Cardiology"
                            }
                        },
                        Experiences = new List<Experience>
                        {
                            new Experience
                            {
                                Id = Guid.NewGuid(),
                                Position = "Resident Physician",
                                StartDate = DateTime.Parse("2008-07-01"),
                                EndDate = DateTime.Parse("2012-06-30"),
                                InstitutionId = Guid.NewGuid()
                            },
                            new Experience
                            {
                                Id = Guid.NewGuid(),
                                Position = "Cardiologist",
                                StartDate = DateTime.Parse("2012-07-01"),
                                EndDate = DateTime.Parse("2020-12-31"),
                                InstitutionId = Guid.NewGuid()
                            }
                        },
                        Specialities = new List<Speciality>
                        {
                            new Speciality
                            {
                                Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3311"),
                                Name = "Cardiology",
                                Description = "Specializes in diagnosing and treating heart diseases."
                            }
                        }
                    },
                    // Doctor Profile 3
                    new DoctorProfile
                    {
                        Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3308"),
                        FullName = "Dr. Sophia Miller",
                        About = "Experienced and caring doctor",
                        Gender = DoctorProfile.GenderType.Female,
                        Email = "sophia.miller@example.com",
                        PhotoId = "photo4",
                        CareerStartTime = DateTime.Parse("2022-01-01T00:00:00Z"),
                        MainInstitutionId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3307"),
                        Institutions = new List<InstitutionProfile>
                        {
                            new InstitutionProfile
                            {
                                Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3307"),
                                InstitutionName = "Wellness Clinic",
                                BranchName = "South Clinic",
                                Website = "http://wellnessclinic.com",
                                PhoneNumber = "123-456-7890",
                                Summary = "Promoting holistic health and well-being.",
                                EstablishedOn = DateTime.Parse("2005-03-10"),
                                Rate = 4.6,
                                LogoId = "logo4",
                                BannerId = "banner4"
                            }
                        },
                        DoctorAvailabilities = new List<DoctorAvailability>
                        {
                            new DoctorAvailability
                            {
                                Id = Guid.NewGuid(),
                                Day = DayOfWeek.Monday,
                                StartTime = "09:00 AM",
                                EndTime = "05:00 PM",
                                InstitutionId = Guid.NewGuid(),
                                SpecialityId = Guid.NewGuid()
                            },
                            new DoctorAvailability
                            {
                                Id = Guid.NewGuid(),
                                Day = DayOfWeek.Thursday,
                                StartTime = "10:00 AM",
                                EndTime = "06:00 PM",
                                InstitutionId = Guid.NewGuid(),
                                SpecialityId = Guid.NewGuid()
                            }
                        },
                        Educations = new List<Education>
                        {
                            new Education
                            {
                                Id = Guid.NewGuid(),
                                EducationInstitution = "Medical University",
                                StartYear = DateTime.Parse("2000-09-01"),
                                GraduationYear = DateTime.Parse("2006-06-30"),
                                Degree = "Doctor of Medicine",
                                FieldOfStudy = "General Medicine"
                            },
                            new Education
                            {
                                Id = Guid.NewGuid(),
                                EducationInstitution = "University of Specialization",
                                StartYear = DateTime.Parse("2006-07-01"),
                                GraduationYear = DateTime.Parse("2011-06-30"),
                                Degree = "Specialization in Pediatrics",
                                FieldOfStudy = "Pediatrics"
                            }
                        },
                        Experiences = new List<Experience>
                        {
                            new Experience
                            {
                                Id = Guid.NewGuid(),
                                Position = "Pediatrician",
                                StartDate = DateTime.Parse("2007-07-01"),
                                EndDate = DateTime.Parse("2015-12-31"),
                                InstitutionId = Guid.NewGuid()
                            },
                            new Experience
                            {
                                Id = Guid.NewGuid(),
                                Position = "Head of Pediatrics Department",
                                StartDate = DateTime.Parse("2016-01-01"),
                                EndDate = DateTime.Parse("2020-12-31"),
                                InstitutionId = Guid.NewGuid()
                            }
                        },
                        Specialities = new List<Speciality>
                        {
                            new Speciality
                            {
                                Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3307"),
                                Name = "Pediatrics",
                                Description = "Specializes in providing medical care for infants, children, and adolescents."
                            }
                        }
                    }

                };



            var doctorProfileRepo = new Mock<IDoctorProfileRepository>();


            doctorProfileRepo.Setup(repo => repo.GetDoctorProfileDetail(It.IsAny<Guid>()))
                .Returns((Guid id) => Task.FromResult(doctorProfiles.FirstOrDefault(d => d.Id == id)));



            doctorProfileRepo.Setup(repo => repo.FilterDoctors(It.IsAny<Guid?>(), It.IsAny<string?>(), It.IsAny<ICollection<string?>>(), It.IsAny<int>(), It.IsAny<string?>(),It.IsAny<int>(),It.IsAny<int>()))
                .Returns((Guid? institutionId, string? name, ICollection<string>? specialityNames, int experienceYears, string? educationInstitutionName,int pageNumber,int pageSize) =>
                {
                    // Set null as the default value for each parameter if they are null or empty
                    institutionId ??= null;
                    specialityNames ??= new List<string>();
                    educationInstitutionName ??= null;
                   

                    var query = doctorProfiles.AsQueryable();

                    if (institutionId != null && institutionId != Guid.Empty)
                    {
                        query = query.Where(d => d.Institutions.Any(i => i.Id == institutionId));
                    }

                    if (specialityNames != null && specialityNames.Any())
                    {
                        query = query.Where(d => d.Specialities.Any(s => specialityNames.Contains(s.Name)));
                    }


                    if (!string.IsNullOrEmpty(educationInstitutionName))
                    {
                        query = query.Where(d => d.Educations.Any(e => e.EducationInstitution == educationInstitutionName));
                    }


                    if (experienceYears > 0)
                    {
                        DateTime startDate = DateTime.Today.AddYears(-experienceYears);
                        query = query.Where(x => x.CareerStartTime <= startDate);
                    }
                    if(pageNumber > 0 && pageSize>0){
                        var skip = (pageNumber-1)*pageSize;
                        return Task.FromResult(query.Skip(skip).Take(pageSize).ToList());
                    }

                    return Task.FromResult(query.ToList());
                });

            doctorProfileRepo.Setup(repo => repo.Add(It.IsAny<DoctorProfile>()))
                .ReturnsAsync((DoctorProfile doctor) =>
                {

                    doctor.Id = new Guid();
                    doctorProfiles.Add(doctor);
                    MockUnitOfWork.changes += 1;
                    MockUnitOfWork.changes += 1;
                    return doctor;

                });


            doctorProfileRepo.Setup(repo => repo.Update(It.IsAny<DoctorProfile>()))
                .Callback((DoctorProfile updatedDoctor) =>
                {
                    var existingDoctor = doctorProfiles.FirstOrDefault(d => d.Id == updatedDoctor.Id);
                    if (existingDoctor != null)
                    {
                        doctorProfiles.Remove(existingDoctor);
                        doctorProfiles.Add(updatedDoctor);
                        MockUnitOfWork.changes += 1;
                    }

                });

            doctorProfileRepo.Setup(repo => repo.Delete(It.IsAny<DoctorProfile>()))
                .Callback((DoctorProfile doctor) =>
                    {
                        var existingDoctor = doctorProfiles.FirstOrDefault(d => d.Id == doctor.Id);
                        if (existingDoctor != null)
                        {
                            doctorProfiles.Remove(existingDoctor);
                            MockUnitOfWork.changes += 1;
                        }
                    });

            doctorProfileRepo.Setup(repo => repo.GetAllDoctors()).ReturnsAsync(doctorProfiles);


            return doctorProfileRepo;
        }
    }

}