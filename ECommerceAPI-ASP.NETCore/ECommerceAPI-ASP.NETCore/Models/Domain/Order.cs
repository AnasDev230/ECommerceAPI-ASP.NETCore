using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public enum OrderStatus
    {
        Pending = 0,
        Paid = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5
    }

    public class Order : BaseEntity
    {
        [Required]
        public string CustomerId { get; set; } = string.Empty;
        public IdentityUser? Customer { get; set; }

        public DateTime? CompletedAt { get; set; }

        [Range(0, 999999.99)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public Guid? BillingAddressId { get; set; }
        public Address? BillingAddress { get; set; }

        [Required]
        [MaxLength(500)]
        public string BillingAddressSnapshot { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string ShippingAddressSnapshot { get; set; } = string.Empty;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public Payment? Payment { get; set; }
        public Shipping? Shipping { get; set; }
    }
}
