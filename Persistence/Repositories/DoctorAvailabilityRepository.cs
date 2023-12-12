using Application.Contracts.Persistence;
using Domain;

namespace Persistence.Repositories
{
    public class DoctorAvailabilityRepository : GenericRepository<DoctorAvailability>, IDoctorAvailabilityRepository
    {

        private readonly HakimHubDbContext _dbContext;

        public DoctorAvailabilityRepository(HakimHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}