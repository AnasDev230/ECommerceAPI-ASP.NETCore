using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart.ShoppingCartItem
{
    public class CreateShoppingCartItemRequestDto
    {
        public Guid StockId { get; set; }
        [Range(1, 7, ErrorMessage = "Quantity must be between 1 and 7.")]
        public int Quantity { get; set; }
    }
}
