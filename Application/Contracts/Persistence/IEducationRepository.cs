using Domain;

namespace Application.Contracts.Persistence;

public interface IEducationRepository : IGenericRepository<Education>
{
        Task<List<Education>> GetAllPopulated();
        Task<Education> GetPopulated(Guid id);
}
