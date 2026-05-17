using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderFromCartAsync(string customerId);
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus);
        Task<bool> DeleteOrderAsync(Guid id);
    }
}
