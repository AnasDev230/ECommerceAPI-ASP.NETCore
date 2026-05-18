namespace ECommerceAPI_ASP.NETCore.Models.DTO.Shipping
{
    public class ShippingDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string? Carrier { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public DateTime? ActualDelivery { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public Guid? ShippingAddressId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
