using Application.Contracts.Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {

        private readonly HakimHubDbContext _dbContext;

        public ServiceRepository(HakimHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<List<Service>> GetServicesByInstitutionId(Guid institutionId)
        {
            var services = await _dbContext.Services
                .Where(s => s.Institutions.Any(i => i.Id == institutionId))
                .ToListAsync();

            return services;
        }
        public async Task<Service> GetServiceByName(string serviceName)
        {
            return await _dbContext.Services.FirstOrDefaultAsync(s => s.ServiceName == serviceName);
        }
    }