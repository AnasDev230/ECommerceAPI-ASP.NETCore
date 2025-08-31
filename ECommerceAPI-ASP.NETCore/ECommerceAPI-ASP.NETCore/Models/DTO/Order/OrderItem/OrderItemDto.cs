namespace ECommerceAPI_ASP.NETCore.Models.DTO.Order.OrderItem
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid StockId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
