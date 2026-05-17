using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? DescriptionPlainText { get; set; }
        public string? SKU { get; set; }
        public string? Brand { get; set; }
        public decimal BasePrice { get; set; }
        public decimal? SalePrice { get; set; }
        public bool IsActive { get; set; }
        public decimal? Weight { get; set; }
        public Guid? ImageID { get; set; }
        public string VendorId { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
