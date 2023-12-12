using Domain;

namespace Application.Contracts.Persistence
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<List<Address>> GetAllPopulated();
        Task<Address> GetPopulated(Guid id);
    }
}