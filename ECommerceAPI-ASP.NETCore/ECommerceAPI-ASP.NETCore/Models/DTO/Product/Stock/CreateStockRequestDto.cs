namespace ECommerceAPI_ASP.NETCore.Models.DTO.Product.Stock
{
    public class CreateStockRequestDto
    {
        public Guid ProductId { get; set; }
        public Guid? ImageID { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
