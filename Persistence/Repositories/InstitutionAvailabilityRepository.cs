using Application.Contracts.Persistence;
using Domain;

namespace Persistence.Repositories
{
    public class InstitutionAvailabilityRepository : GenericRepository<InstitutionAvailability>, IInstitutionAvailabilityRepository
    {

        private readonly HakimHubDbContext _dbContext;

        public InstitutionAvailabilityRepository(HakimHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}