using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface IAddressRepository
    {
        Task<Address> CreateAsync(Address address);
        Task<IEnumerable<Address>> GetAllByUserIdAsync(string userId);
        Task<Address?> GetByIdAsync(Guid id);
        Task<Address?> GetDefaultByUserIdAsync(string userId);
        Task<bool> UpdateAsync(Address address);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SetDefaultAsync(Guid addressId, string userId);
    }
}
