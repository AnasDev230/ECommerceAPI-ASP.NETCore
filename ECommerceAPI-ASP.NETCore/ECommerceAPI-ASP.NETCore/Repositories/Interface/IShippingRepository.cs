using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface IShippingRepository
    {
        Task<Shipping> CreateAsync(Shipping shipping);
        Task<Shipping?> GetByIdAsync(Guid id);
        Task<Shipping?> GetByOrderIdAsync(Guid orderId);
        Task<Shipping?> GetByTrackingNumberAsync(string trackingNumber);
        Task<IEnumerable<Shipping>> GetByStatusAsync(ShippingStatus status);
        Task<IEnumerable<Shipping>> GetAllAsync();
        Task<Shipping?> UpdateStatusAsync(Guid shippingId, ShippingStatus newStatus);
        Task<Shipping?> UpdateAsync(Shipping shipping);
        Task<Shipping?> DeleteAsync(Guid id);
    }
}
