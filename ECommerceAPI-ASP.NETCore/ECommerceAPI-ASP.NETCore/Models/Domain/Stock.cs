using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class Stock : BaseEntity
    {
        [Required]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        [MaxLength(50)]
        public string SKU { get; set; } = string.Empty;

        public Guid? ImageID { get; set; }
        public Image? Image { get; set; }

        [MaxLength(50)]
        public string? Color { get; set; }

        [MaxLength(50)]
        public string? Size { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } = 0;

        [Range(0, 999999.99)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public int LowStockThreshold { get; set; } = 10;

        public int ReservedQuantity { get; set; } = 0;
    }
}
