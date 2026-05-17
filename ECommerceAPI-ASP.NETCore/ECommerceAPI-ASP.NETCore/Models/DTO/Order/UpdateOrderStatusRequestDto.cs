using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Order
{
    public class UpdateOrderStatusRequestDto
    {
        public OrderStatus Status { get; set; }
    }
}
