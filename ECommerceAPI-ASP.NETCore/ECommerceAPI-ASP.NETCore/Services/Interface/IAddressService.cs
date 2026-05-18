using ECommerceAPI_ASP.NETCore.Models.DTO.Address;

namespace ECommerceAPI_ASP.NETCore.Services.Interface
{
    public interface IAddressService
    {
        Task<AddressDto> CreateAsync(string userId, CreateAddressRequestDto request);
        Task<IEnumerable<AddressDto>> GetAllByUserIdAsync(string userId);
        Task<AddressDto?> GetByIdAsync(Guid id, string userId);
        Task<AddressDto?> GetDefaultAsync(string userId);
        Task<AddressDto?> UpdateAsync(Guid id, string userId, UpdateAddressRequestDto request);
        Task<bool> DeleteAsync(Guid id, string userId);
        Task<AddressDto?> SetDefaultAsync(Guid addressId, string userId);
    }
}
