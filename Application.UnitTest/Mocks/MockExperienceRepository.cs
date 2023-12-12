using Application.Contracts.Persistence;
using Domain;
using Moq;

namespace Application.UnitTest.Mocks;

public static class MockExperienceRepository
{
    public static Mock<IExperienceRepository> GetExperienceRepository()
    {
        var experiences = new List<Experience>
        {
            new ()
            {
                    Id = Guid.NewGuid(),
                    Position = "position 1",
                    Description = "Description 1",
                    StartDate = DateTime.MinValue,
                    EndDate = DateTime.MaxValue,
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid()
        
            },
            
            new ()
            {
                    Id = Guid.NewGuid(),
                    Position = "position 2",
                    Description = "Description 2",
                    StartDate = DateTime.MinValue,
                    EndDate = DateTime.MaxValue,
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid()
                    
            }
        };

        var mockRepo = new Mock<IExperienceRepository>();
        
        mockRepo.Setup(r => r.Add(It.IsAny<Experience>())).ReturnsAsync((Experience experience) =>
        {
            experiences.Add(experience);
            return experience; 
        });

        mockRepo.Setup(r => r.Update(It.IsAny<Domain.Experience>())).Callback((Experience experience) =>
        {
            var newExperience = experiences.Where((r) => r.Id != experience.Id);
            experiences = newExperience.ToList();
            experiences.Add(experience);
        });
        
        mockRepo.Setup(r => r.Delete(It.IsAny<Domain.Experience>())).Callback((Experience experience) =>
        {
            if (experiences.Exists(b => b.Id == experience.Id))
                experiences.Remove(experiences.Find(b => b.Id == experience.Id)!);
        });


        return mockRepo;
    }
}