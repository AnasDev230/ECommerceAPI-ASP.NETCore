using ECommerceAPI_ASP.NETCore.Models.DTO.Order.OrderItem;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Order
{
    public class CreateOrderRequestDto
    {
        public ICollection<CreateOrderItemRequestDto> Items { get; set; }
    }
}
