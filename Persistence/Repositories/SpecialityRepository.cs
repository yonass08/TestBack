using Application.Contracts.Persistence;
using Domain;

namespace Persistence.Repositories
{
    public class SpecialityRepository : GenericRepository<Speciality>, ISpecialityRepository
    {

        private readonly HakimHubDbContext _dbContext;

        public SpecialityRepository(HakimHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}