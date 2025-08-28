namespace ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart.ShoppingCartItem
{
    public class ShoppingCartItemDto
    {
        public Guid Id { get; set; }
        public Guid ShoppingCartId { get; set; }
        public Guid StockId { get; set; }
        public int Quantity { get; set; }
    }
}
