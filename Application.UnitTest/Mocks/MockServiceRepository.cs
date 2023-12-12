using Application.Contracts.Persistence;
using Domain;
using Moq;

namespace Application.UnitTest.Mocks;
public static class MockServiceRepository
{
    public static Mock<IServiceRepository> GetServiceRepository()
    {
        var services = new List<Service>
        {
            new ()
            {
                    Id = Guid.NewGuid(),
                    ServiceName = "Service 1",
                    ServiceDescription = "Service description",                  
            },
            
            new ()
            {
                    Id = Guid.NewGuid(),
                    ServiceName = "Service 2",
                    ServiceDescription = "Service description2", 
            }
        };
    
        
        var mockRepo = new Mock<IServiceRepository>();

        
        mockRepo.Setup(r => r.Add(It.IsAny<Service>())).ReturnsAsync((Service service) =>
        {
            services.Add(service);
            return service; 
        });

        mockRepo.Setup(r => r.Update(It.IsAny<Service>())).Callback((Service service) =>
        {
            var newService = services.Where((r) => r.Id != service.Id);
            services = newService.ToList();
            services.Add(service);
        });
        
        mockRepo.Setup(r => r.Delete(It.IsAny<Service>())).Callback((Service service) =>
        {
            if (services.Exists(b => b.Id == service.Id))
                services.Remove(services.Find(b => b.Id == service.Id)!);
        });

        return mockRepo;
    }
}