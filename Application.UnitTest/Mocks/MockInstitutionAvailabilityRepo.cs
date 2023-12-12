using Application.Contracts.Persistence;
using Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UnitTest.Mocks
{
    public static class MockInstitutionAvailabilityRepository
    {
        public static Mock<IInstitutionAvailabilityRepository> GetInstitutionAvailabilityRepository()
        {
            var InstitutionAvailabilities = new List<InstitutionAvailability>
            {
                new InstitutionAvailability
                {
                    StartDay = DayOfWeek.Monday,
                    EndDay = DayOfWeek.Sunday,
                    Opening = "2:00AM",
                    Closing = "4:00PM",
                    TwentyFourHours = true,
                    InstitutionId = Guid.NewGuid(),
                    Id = Guid.NewGuid()
                },

                new InstitutionAvailability
                {
                    StartDay = DayOfWeek.Monday,
                    EndDay = DayOfWeek.Sunday,
                    Opening = "1:00AM",
                    Closing = "7:00PM",
                    TwentyFourHours = false,
                    InstitutionId = Guid.NewGuid(),
                    Id = Guid.NewGuid()
                }
            };

            var mockRepo = new Mock<IInstitutionAvailabilityRepository>();

            mockRepo.Setup(r => r.GetAll()).ReturnsAsync(InstitutionAvailabilities);

            mockRepo.Setup(r => r.Add(It.IsAny<InstitutionAvailability>())).ReturnsAsync((InstitutionAvailability institutionAvailability) =>
            {
                institutionAvailability.Id = Guid.NewGuid();
                InstitutionAvailabilities.Add(institutionAvailability);
                return institutionAvailability;
            });

            mockRepo.Setup(r => r.Update(It.IsAny<InstitutionAvailability>())).Callback((InstitutionAvailability institutionAvailability) =>
            {
                var existingAvailability = InstitutionAvailabilities.FirstOrDefault(r => r.Id == institutionAvailability.Id);
                if (existingAvailability != null)
                {
                    existingAvailability.StartDay = institutionAvailability.StartDay;
                    existingAvailability.EndDay = institutionAvailability.EndDay;
                    existingAvailability.Opening = institutionAvailability.Opening;
                    existingAvailability.Closing = institutionAvailability.Closing;
                    existingAvailability.TwentyFourHours = institutionAvailability.TwentyFourHours;
                    existingAvailability.InstitutionId = institutionAvailability.InstitutionId;
                }
            });

            mockRepo.Setup(r => r.Delete(It.IsAny<InstitutionAvailability>())).Callback((InstitutionAvailability institutionAvailability) =>
            {
                var existingAvailability = InstitutionAvailabilities.FirstOrDefault(r => r.Id == institutionAvailability.Id);
                if (existingAvailability != null)
                {
                    InstitutionAvailabilities.Remove(existingAvailability);
                }
            });

            mockRepo.Setup(r => r.Exists(It.IsAny<Guid>())).ReturnsAsync((Guid id) =>
            {
                return InstitutionAvailabilities.Any(r => r.Id == id);
            });

            mockRepo.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Guid id) =>
            {
                return InstitutionAvailabilities.FirstOrDefault(r => r.Id == id);
            });

            return mockRepo;
        }
    }
}



















