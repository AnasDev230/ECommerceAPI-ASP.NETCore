using ECommerceAPI_ASP.NETCore.Models.DTO.Order.OrderItem;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }
    }
}
