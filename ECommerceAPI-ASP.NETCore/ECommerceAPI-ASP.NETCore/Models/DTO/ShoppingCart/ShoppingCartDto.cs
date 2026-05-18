namespace ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
