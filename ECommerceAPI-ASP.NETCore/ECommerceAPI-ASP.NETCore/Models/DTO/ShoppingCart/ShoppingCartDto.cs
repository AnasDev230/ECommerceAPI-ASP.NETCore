using ECommerceAPI_ASP.NETCore.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
