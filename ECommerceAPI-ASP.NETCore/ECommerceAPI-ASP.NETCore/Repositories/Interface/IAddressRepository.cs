using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface IAddressRepository
    {
        Task<Address> CreateAsync(Address address);
        Task<IEnumerable<Address>> GetAllByUserIdAsync(string userId);
        Task<Address?> GetByIdAsync(Guid id);
        Task<Address?> GetDefaultByUserIdAsync(string userId);
        Task<Address?> UpdateAsync(Address address);
        Task<Address?> DeleteAsync(Guid id);
        Task<Address?> SetDefaultAsync(Guid addressId, string userId);
    }
}
