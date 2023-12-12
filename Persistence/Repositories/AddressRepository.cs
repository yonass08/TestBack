using Application.Contracts.Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {

        private readonly HakimHubDbContext _dbContext;

        public AddressRepository(HakimHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Address>> GetAllPopulated()
        {
            return await _dbContext.Set<Address>()
                .Include(x => x.Institution)
                .AsNoTracking().ToListAsync();
        }

        public async Task<Address> GetPopulated(Guid id)
        {
            return await _dbContext.Set<Address>()
                .Include(x => x.Institution)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

    }
}