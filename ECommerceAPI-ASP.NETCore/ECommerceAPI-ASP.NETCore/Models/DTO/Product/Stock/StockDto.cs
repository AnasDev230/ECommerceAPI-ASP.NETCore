using Blog_API.Models.DTO;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Product.Stock
{
    public class StockDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public Guid? ImageID { get; set; }
        public ImageDto Image { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
