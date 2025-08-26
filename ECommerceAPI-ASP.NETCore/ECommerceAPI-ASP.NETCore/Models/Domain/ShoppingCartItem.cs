namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class ShoppingCartItem
    {
        public Guid Id { get; set; }

        public Guid ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        public Guid StockId { get; set; }
        public Stock Stock { get; set; }

        public int Quantity { get; set; }
    }
}
