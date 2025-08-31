namespace ECommerceAPI_ASP.NETCore.Models.DTO.Order.OrderItem
{
    public class CreateOrderItemRequestDto
    {
        public Guid StockId { get; set; }
        public int Quantity { get; set; }
    }
}
