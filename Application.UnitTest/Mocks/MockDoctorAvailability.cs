using Application.Contracts.Persistence;
using Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UnitTest.Mocks
{
    public static class MockDoctorAvailabilityRepository
    {
        public static Mock<IDoctorAvailabilityRepository> GetDoctorAvailabilityRepository()
        {
            var DoctorAvailabilities = new List<DoctorAvailability>
            {
                new DoctorAvailability
                {
                    
                    Id = Guid.NewGuid(),
                    Day = DayOfWeek.Friday,
                    StartTime = "2:00 AM",
                    EndTime = "3:00 PM",
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid(),
                    SpecialityId = Guid.NewGuid(),
        
                    
                },

                new DoctorAvailability
                {
                    Id = Guid.NewGuid(),
                    Day = DayOfWeek.Friday,
                    StartTime = "2:00 AM",
                    EndTime = "3:00 PM",
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid(),
                    SpecialityId = Guid.NewGuid(),
                }
            };

            var mockRepo = new Mock<IDoctorAvailabilityRepository>();

            mockRepo.Setup(r => r.GetAll()).ReturnsAsync(DoctorAvailabilities);

            mockRepo.Setup(r => r.Add(It.IsAny<DoctorAvailability>())).ReturnsAsync((DoctorAvailability doctorAvailability) =>
            {
                doctorAvailability.Id = Guid.NewGuid();
                DoctorAvailabilities.Add(doctorAvailability);
                return doctorAvailability;
            });

            mockRepo.Setup(r => r.Update(It.IsAny<DoctorAvailability>())).Callback((DoctorAvailability doctorAvailability) =>
            {
                var existingAvailability = DoctorAvailabilities.FirstOrDefault(r => r.Id == doctorAvailability.Id);
                if (existingAvailability != null)
                {
                    existingAvailability.Day = doctorAvailability.Day;
                    existingAvailability.StartTime = doctorAvailability.StartTime;
                    existingAvailability.EndTime = doctorAvailability.EndTime;
                    existingAvailability.InstitutionId = doctorAvailability.InstitutionId;
                    existingAvailability.DoctorId = doctorAvailability.DoctorId;
                }
            });

            mockRepo.Setup(r => r.Delete(It.IsAny<DoctorAvailability>())).Callback((DoctorAvailability doctorAvailability) =>
            {
                var existingAvailability = DoctorAvailabilities.FirstOrDefault(r => r.Id == doctorAvailability.Id);
                if (existingAvailability != null)
                {
                    DoctorAvailabilities.Remove(existingAvailability);
                }
            });

            mockRepo.Setup(r => r.Exists(It.IsAny<Guid>())).ReturnsAsync((Guid id) =>
            {
                return DoctorAvailabilities.Any(r => r.Id == id);
            });

            mockRepo.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Guid id) =>
            {
                return DoctorAvailabilities.FirstOrDefault(r => r.Id == id);
            });

            return mockRepo;
        }
    }
}



















