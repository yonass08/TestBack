using Application.Contracts.Persistence;
using Application.UnitTest.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTest.Mocks
{
    public static class MockUnitOfWork
    {
        public static int changes = 0;

        public static Mock<IUnitOfWork> GetUnitOfWork()
        {
            var mockUow = new Mock<IUnitOfWork>();
          

            var mockSpecialityRepo = MockSpecialityRepository.GetSpecialityRepository();
            var mockEducationRepo = MockEducationRepository.GetEducationRepository();
            var mockInstitutionAvailabilityRepo =  MockInstitutionAvailabilityRepository.GetInstitutionAvailabilityRepository();
            var mockDoctorAvailabilityRepo = MockDoctorAvailabilityRepository.GetDoctorAvailabilityRepository();
            var mockAddressRepo = MockAddressRepository.GetAddressRepository();
            
            var mockInstitutionProfileRepo = MockInstitutionProfileRepository.GetInstitutionProfileRepository();

            mockUow.Setup(r => r.InstitutionAvailabilityRepository).Returns(mockInstitutionAvailabilityRepo.Object);
            mockUow.Setup(r => r.DoctorAvailabilityRepository).Returns(mockDoctorAvailabilityRepo.Object);
            mockUow.Setup(r => r.AddressRepository).Returns(mockAddressRepo.Object);
            mockUow.Setup(r => r.InstitutionProfileRepository).Returns(mockInstitutionProfileRepo.Object);
            mockUow.Setup(r => r.EducationRepository).Returns(mockEducationRepo.Object);
            mockUow.Setup(r => r.SpecialityRepository).Returns(mockSpecialityRepo.Object);

            mockUow.Setup(r => r.Save()).ReturnsAsync(() =>
            {
                var temp = changes;
                changes = 0;
                return temp;
            });

            var mockDoctorProfileRepo = MockDoctorProfileRepository.GetDoctorProfileRepository();
            mockUow.Setup(uow=>uow.DoctorProfileRepository).Returns(mockDoctorProfileRepo.Object);
            return mockUow;
        }
    }
}

