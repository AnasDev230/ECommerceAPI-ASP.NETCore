namespace ECommerceAPI_ASP.NETCore.Models.DTO.Address
{
    public class AddressDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public bool IsDefault { get; set; }
        public string AddressType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
