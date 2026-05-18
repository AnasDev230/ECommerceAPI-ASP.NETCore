namespace ECommerceAPI_ASP.NETCore.Models.DTO.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? DescriptionPlainText { get; set; }
        public string? Brand { get; set; }
        public bool IsActive { get; set; }
        public decimal? Weight { get; set; }
        public Guid? ImageID { get; set; }
        public string VendorId { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
