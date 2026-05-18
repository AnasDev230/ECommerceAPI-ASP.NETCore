using ECommerceAPI_ASP.NETCore.Models.DTO.Order.OrderItem;
using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
