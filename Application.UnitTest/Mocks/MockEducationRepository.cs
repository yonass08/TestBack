using Application.Contracts.Persistence;
using Domain;
using Moq;

namespace Application.UnitTest.Mocks;

public class MockEducationRepository
{   public static Mock<IEducationRepository> GetEducationRepository()
    {
        var educations = new List<Education>
        {
           new Education
                {
                    Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                    EducationInstitution = "Addis Ababa University",
                    StartYear = DateTime.Today,
                    GraduationYear = DateTime.Today,
                    Degree = "Masters",
                    DoctorId = Guid.NewGuid(),
                    EducationInstitutionLogoId = "Addis Ababa Logo"
                },
           new Education
                {
                    Id = Guid.NewGuid(),
                    EducationInstitution = "Arba Minch University",
                    StartYear = DateTime.Now,
                    GraduationYear = DateTime.Today,
                    Degree = "Bachelors",
                    DoctorId = Guid.NewGuid(),
                    EducationInstitutionLogoId = "Addis Ababa Logo"
                }
        };

        var mockRepository = new Mock<IEducationRepository>();

        mockRepository.Setup(r => r.GetAll()).ReturnsAsync(educations);
        mockRepository.Setup(r => r.GetAllPopulated()).ReturnsAsync(educations);

        mockRepository.Setup(r => r.Add(It.IsAny<Education>())).ReturnsAsync((Education edu) =>
        {
            edu.Id = Guid.NewGuid();
            educations.Add(edu);
            MockUnitOfWork.changes += 1;
            return edu;
        });

        mockRepository.Setup(r => r.Update(It.IsAny<Education>())).Callback((Education edu) =>
        {
            var newEdu = educations.Where((r) => r.Id != edu.Id);
            educations = newEdu.ToList();
            educations.Add(edu);
            MockUnitOfWork.changes += 1;
        });

        mockRepository.Setup(r => r.Delete(It.IsAny<Education>())).Callback((Education chore) =>
        {
            if (educations.Exists(b => b.Id == chore.Id)){
                educations.Remove(educations.Find(b => b.Id == chore.Id)!);
                MockUnitOfWork.changes -= 1;
            }
                
        });

        mockRepository.Setup(r => r.Exists(It.IsAny<Guid>())).ReturnsAsync((Guid id) =>
        {
            var chore = educations.FirstOrDefault((r) => r.Id == id);
            return chore != null;
        });

        mockRepository.Setup(r => r.Get(It.IsAny<Guid>()))!.ReturnsAsync((Guid id) =>
        {
            return educations.FirstOrDefault((r) => r.Id == id);
        });

        return mockRepository;
    }
}