using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class Rating : BaseEntity
    {
        [Required]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        public string CustomerId { get; set; } = string.Empty;
        public IdentityUser? Customer { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

        [MaxLength(2000)]
        public string? Comment { get; set; }

        public bool IsVerifiedPurchase { get; set; } = false;
    }
}
