using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Contracts.Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class EducationRepository : GenericRepository<Education>, IEducationRepository
{

    private readonly HakimHubDbContext _dbContext;

    public EducationRepository(HakimHubDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Education>> GetAllPopulated()
        {
            return await _dbContext.Set<Education>()
                .Include(x => x.EducationInstitutionLogo)
                .AsNoTracking().ToListAsync();
        }
    
    public async Task<Education> GetPopulated(Guid id)
        {
            return await _dbContext.Set<Education>()
                .Include(x => x.EducationInstitutionLogo)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
}
