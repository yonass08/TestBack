using Domain;

namespace Application.Contracts.Persistence;

public interface IServiceRepository : IGenericRepository<Service>
{
    Task<List<Service>> GetServicesByInstitutionId(Guid institutionId);
    Task<Service> GetServiceByName(string serviceName);

}
