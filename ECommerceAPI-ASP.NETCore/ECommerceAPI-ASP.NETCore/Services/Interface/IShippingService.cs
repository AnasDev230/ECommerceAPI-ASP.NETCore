using ECommerceAPI_ASP.NETCore.Models.DTO.Shipping;

namespace ECommerceAPI_ASP.NETCore.Services.Interface
{
    public interface IShippingService
    {
        Task<ShippingDto> CreateAsync(CreateShippingRequestDto request);
        Task<ShippingDto?> GetByIdAsync(Guid id);
        Task<ShippingDto?> GetByOrderIdAsync(Guid orderId);
        Task<ShippingDto?> GetByTrackingNumberAsync(string trackingNumber);
        Task<IEnumerable<ShippingDto>> GetAllAsync();
        Task<IEnumerable<ShippingDto>> GetByStatusAsync(string status);
        Task<ShippingDto?> UpdateAsync(Guid id, UpdateShippingRequestDto request);
        Task<ShippingDto?> UpdateStatusAsync(Guid shippingId, UpdateShippingStatusRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
