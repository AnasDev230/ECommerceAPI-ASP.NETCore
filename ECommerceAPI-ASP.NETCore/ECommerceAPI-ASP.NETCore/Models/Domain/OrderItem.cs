using System.ComponentModel.DataAnnotations;

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

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, 999999.99)]
        public decimal UnitPrice { get; set; }

        [Range(0, 999999.99)]
        public decimal TotalPrice { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
