namespace ECommerceAPI_ASP.NETCore.Models.DTO.Product.Stock
{
    public class StockDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
