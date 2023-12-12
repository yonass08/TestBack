
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Contracts.Persistence;
using Domain;
using Domain.Common;
using Moq;

namespace Application.UnitTest.Mocks
{
    public class MockSpecialityRepository
    {
        public static Mock<ISpecialityRepository> GetSpecialityRepository()
        {
            var specialities = new List<Speciality>
            {
                new Speciality
                {
                    Id = Guid.NewGuid(),
                    Name = "Speciality 1",
                    Description = "Description 1"
                },
                new Speciality
                {
                    Id = Guid.NewGuid(),
                    Name = "Speciality 2",
                    Description = "Description 2"
                }
            };

            var mockRepository = new Mock<ISpecialityRepository>();

            mockRepository.Setup(r => r.GetAll()).ReturnsAsync(specialities);

            mockRepository.Setup(r => r.Add(It.IsAny<Speciality>())).ReturnsAsync((Speciality speciality) =>
            {
                speciality.Id = Guid.NewGuid();
                specialities.Add(speciality);
                MockUnitOfWork.changes += 1;
                return speciality;
            });

            mockRepository.Setup(r => r.Update(It.IsAny<Speciality>())).Callback((Speciality speciality) =>
            {
                var existingSpeciality = specialities.FirstOrDefault(s => s.Id == speciality.Id);
                if (existingSpeciality != null)
                {
                    existingSpeciality.Name = speciality.Name;
                    existingSpeciality.Description = speciality.Description;
                }
            });

            mockRepository.Setup(r => r.Delete(It.IsAny<Speciality>())).Callback((Speciality speciality) =>
            {
                var existingSpeciality = specialities.FirstOrDefault(s => s.Id == speciality.Id);
                if (existingSpeciality != null)
                {
                    specialities.Remove(existingSpeciality);
                }
            });

            mockRepository.Setup(r => r.Exists(It.IsAny<Guid>())).ReturnsAsync((Guid id) =>
            {
                return specialities.Any(s => s.Id == id);
            });

            mockRepository.Setup(r => r.Get(It.IsAny<Guid>()))!.ReturnsAsync((Guid id) =>
                {
                    return specialities.FirstOrDefault((r) => r.Id == id);
                });

            return mockRepository;
        }
    }
}
