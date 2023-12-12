using Domain;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Moq;

namespace Application.UnitTest.Mocks
{
    public static class MockAddressRepository
    {
        public static Mock<IAddressRepository> GetAddressRepository()
        {
            var addresses = new List<Address>
            {
                new Address
                {
                    Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    Country = "Country 1",
                    Region = "Region 1",
                    Zone = "Zone 1",
                    Woreda = "Woreda 1",
                    City = "City 1",
                    SubCity = "SubCity 1",
                    Summary = "Summary 1",
                    Longitude = 1.0,
                    Latitude = 1.0,
                    InstitutionId = Guid.NewGuid()
                },
                new Address
                {
                    Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                    Country = "Country 2",
                    Region = "Region 2",
                    Zone = "Zone 2",
                    Woreda = "Woreda 2",
                    City = "City 2",
                    SubCity = "SubCity 2",
                    Summary = "Summary 2",
                    Longitude = 2.0,
                    Latitude = 2.0,
                    InstitutionId = Guid.NewGuid()
                }
            };

            var mockRepo = new Mock<IAddressRepository>();

            mockRepo.Setup(r => r.GetAll()).ReturnsAsync(addresses);
            mockRepo.Setup(r => r.GetAllPopulated()).ReturnsAsync(addresses);


            mockRepo.Setup(r => r.Add(It.IsAny<Address>())).ReturnsAsync((Address address) =>
            {
                address.Id = Guid.NewGuid();
                addresses.Add(address);
                MockUnitOfWork.changes += 1;
                return address;
            });

            mockRepo.Setup(r => r.Update(It.IsAny<Address>())).Callback((Address address) =>
            {
                var existingAddress = addresses.FirstOrDefault(a => a.Id == address.Id);
                if (existingAddress != null)
                {
                    existingAddress.Country = address.Country;
                    existingAddress.Region = address.Region;
                    existingAddress.Zone = address.Zone;
                    existingAddress.Woreda = address.Woreda;
                    existingAddress.City = address.City;
                    existingAddress.SubCity = address.SubCity;
                    existingAddress.Summary = address.Summary;
                    existingAddress.Longitude = address.Longitude;
                    existingAddress.Latitude = address.Latitude;
                    existingAddress.InstitutionId = address.InstitutionId;
                }
            });

            mockRepo.Setup(r => r.Delete(It.IsAny<Address>())).Callback((Address address) =>
            {
                addresses.RemoveAll(a => a.Id == address.Id);
            });

            mockRepo.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Guid id) =>
            {
                return addresses.FirstOrDefault(a => a.Id == id);
            });

            return mockRepo;
        }
    }
}
