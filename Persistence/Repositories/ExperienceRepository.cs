using Application.Contracts.Persistence;
using Domain;

namespace Persistence.Repositories;

public class ExperienceRepository : GenericRepository<Experience>, IExperienceRepository
    {

        private readonly HakimHubDbContext _dbContext;

        public ExperienceRepository(HakimHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
