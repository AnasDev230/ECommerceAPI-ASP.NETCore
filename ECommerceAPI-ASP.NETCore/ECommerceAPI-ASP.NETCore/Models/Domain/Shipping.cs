using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class Shipping : BaseEntity
    {
        [Required]
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        [MaxLength(100)]
        public string? Carrier { get; set; }

        [MaxLength(100)]
        public string? TrackingNumber { get; set; }

        public DateTime? EstimatedDelivery { get; set; }

        public DateTime? ActualDelivery { get; set; }

        public ShippingStatus Status { get; set; } = ShippingStatus.Pending;

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public Guid? ShippingAddressId { get; set; }
        public Address? ShippingAddress { get; set; }
    }

    public enum ShippingStatus
    {
        Pending = 0,
        LabelCreated = 1,
        InTransit = 2,
        OutForDelivery = 3,
        Delivered = 4,
        Failed = 5,
        Returned = 6
    }
}