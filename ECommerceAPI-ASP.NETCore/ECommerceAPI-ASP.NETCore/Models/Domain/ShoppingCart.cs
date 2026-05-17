using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class ShoppingCart : BaseEntity
    {
        [Required]
        public string CustomerId { get; set; } = string.Empty;
        public IdentityUser? Customer { get; set; }

        public new DateTime? UpdatedAt { get; set; }

        public ICollection<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    }
}
