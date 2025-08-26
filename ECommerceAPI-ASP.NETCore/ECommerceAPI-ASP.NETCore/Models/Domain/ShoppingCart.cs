using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public IdentityUser Customer { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ShoppingCartItem> Items { get; set; }
    }
}
