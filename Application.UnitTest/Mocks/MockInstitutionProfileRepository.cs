using Application.Contracts.Persistence;
using Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UnitTest.Mocks
{
    public static class MockInstitutionProfileRepository
    {
        public static Mock<IInstitutionProfileRepository> GetInstitutionProfileRepository()
        {
            var institutionProfiles = new List<InstitutionProfile>
            {
                new InstitutionProfile
                {
                    Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    InstitutionName = "Institution 1",
                    BranchName = "Branch 1",
                    Website = "www.Website.com",
                    PhoneNumber = "Phone 1",
                    Summary = "Summary 1",
                    EstablishedOn = DateTime.Now.AddDays(-10),
                    Rate = 4.5,
                    LogoId = "LogoId 1",
                    BannerId = "BannerId 1"
                },
                new InstitutionProfile
                {
                    Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                    InstitutionName = "Institution 2",
                    BranchName = "Branch 2",
                    Website = "www.Website.com",
                    PhoneNumber = "Phone 2",
                    Summary = "Summary 2",
                    EstablishedOn = DateTime.Now.AddDays(-10),
                    Rate = 3.8,
                    LogoId = "LogoId 2",
                    BannerId = "BannerId 2"
                }
            };

            var mockRepo = new Mock<IInstitutionProfileRepository>();

            mockRepo.Setup(r => r.GetAll()).ReturnsAsync(institutionProfiles);

            mockRepo.Setup(r => r.Add(It.IsAny<InstitutionProfile>())).ReturnsAsync((InstitutionProfile institutionProfile) =>
            {
                institutionProfile.Id = Guid.NewGuid();
                institutionProfiles.Add(institutionProfile);
                MockUnitOfWork.changes += 1;
                return institutionProfile;
            });

            mockRepo.Setup(r => r.Update(It.IsAny<InstitutionProfile>())).Callback((InstitutionProfile profile) =>
            {
                var newProfiles = institutionProfiles.Where((r) => r.Id != profile.Id);
                institutionProfiles = newProfiles.ToList();
                institutionProfiles.Add(profile);
                MockUnitOfWork.changes += 1;
            });

            mockRepo.Setup(r => r.Delete(It.IsAny<InstitutionProfile>())).Callback((InstitutionProfile institutionProfile) =>
            {
                var existingProfile = institutionProfiles.FirstOrDefault(p => p.Id == institutionProfile.Id);
                if (existingProfile != null)
                {
                    institutionProfiles.Remove(existingProfile);
                }
            });

            mockRepo.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Guid Id) =>
            {
                MockUnitOfWork.changes += 1;
                return institutionProfiles.FirstOrDefault((r) => r.Id == Id);
            });

            mockRepo.Setup(r => r.GetPopulatedInstitution(It.IsAny<Guid>())).ReturnsAsync((Guid Id) =>
            {
                MockUnitOfWork.changes += 1;
                return institutionProfiles.FirstOrDefault((r) => r.Id == Id);
            });

            return mockRepo;
        }
    }
}
