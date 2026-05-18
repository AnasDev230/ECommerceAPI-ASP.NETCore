using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Order;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<OrderDto> CreateFromCartAsync(string customerId)
        {
            var order = await orderRepository.CreateOrderFromCartAsync(customerId);
            return mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto?> GetByIdAsync(Guid id)
        {
            var order = await orderRepository.GetOrderByIdAsync(id);
            if (order == null)
                return null;

            return mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto?> GetByIdAndCustomerAsync(Guid id, string customerId)
        {
            var order = await orderRepository.GetOrderByIdAsync(id);
            if (order == null || order.CustomerId != customerId)
                return null;

            return mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await orderRepository.GetAllOrdersAsync();
            return mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetByCustomerIdAsync(string customerId)
        {
            var orders = await orderRepository.GetOrdersByCustomerIdAsync(customerId);
            return mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto?> UpdateStatusAsync(Guid orderId, UpdateOrderStatusRequestDto request)
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return null;

            if (order.Status is OrderStatus.Shipped or OrderStatus.Delivered or OrderStatus.Cancelled)
                throw new InvalidOperationException("Order status cannot be updated from its current state.");

            var updated = await orderRepository.UpdateOrderStatusAsync(orderId, request.Status);
            if (!updated)
                return null;

            var updatedOrder = await orderRepository.GetOrderByIdAsync(orderId);
            return mapper.Map<OrderDto>(updatedOrder);
        }

        public async Task<bool> DeleteAsync(Guid orderId, string customerId)
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);
            if (order == null || order.CustomerId != customerId)
                return false;

            return await orderRepository.DeleteOrderAsync(orderId);
        }
    }
}
