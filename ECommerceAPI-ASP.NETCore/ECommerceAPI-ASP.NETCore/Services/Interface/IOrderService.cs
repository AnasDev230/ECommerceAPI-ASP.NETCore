using ECommerceAPI_ASP.NETCore.Models.DTO.Order;

namespace ECommerceAPI_ASP.NETCore.Services.Interface
{
    public interface IOrderService
    {
        Task<OrderDto> CreateFromCartAsync(string customerId);
        Task<OrderDto?> GetByIdAsync(Guid id);
        Task<OrderDto?> GetByIdAndCustomerAsync(Guid id, string customerId);
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<IEnumerable<OrderDto>> GetByCustomerIdAsync(string customerId);
        Task<OrderDto?> UpdateStatusAsync(Guid orderId, UpdateOrderStatusRequestDto request);
        Task<bool> DeleteAsync(Guid orderId, string customerId);
    }
}
