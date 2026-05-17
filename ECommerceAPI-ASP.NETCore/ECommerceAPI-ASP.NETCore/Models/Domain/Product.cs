using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(8000)]
        public string? DescriptionPlainText { get; set; }

        [MaxLength(100)]
        public string? SKU { get; set; }

        [MaxLength(100)]
        public string? Brand { get; set; }

        [Range(0, 999999.99)]
        public decimal BasePrice { get; set; }

        [Range(0, 999999.99)]
        public decimal? SalePrice { get; set; }

        public bool IsActive { get; set; } = true;

        [Range(0, 999999.999)]
        public decimal? Weight { get; set; }

        public Guid? ImageID { get; set; }
        public Image? Image { get; set; }

        [Required]
        public string VendorId { get; set; } = string.Empty;
        public IdentityUser? Vendor { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
