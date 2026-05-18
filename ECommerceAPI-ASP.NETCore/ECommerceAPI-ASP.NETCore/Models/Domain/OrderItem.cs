using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class OrderItem : BaseEntity
    {
        [Required]
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        [Required]
        public Guid StockId { get; set; }
        public Stock? Stock { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductNameSnapshot { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? VariantDetailsSnapshot { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, 999999.99)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice => Quantity * UnitPrice;

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
