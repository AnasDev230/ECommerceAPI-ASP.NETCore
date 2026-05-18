using System.ComponentModel.DataAnnotations;
using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Shipping
{
    public class UpdateShippingRequestDto
    {
        [MaxLength(100)]
        public string? Carrier { get; set; }

        [MaxLength(100)]
        public string? TrackingNumber { get; set; }

        public DateTime? EstimatedDelivery { get; set; }

        public DateTime? ActualDelivery { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public Guid? ShippingAddressId { get; set; }
    }
}
