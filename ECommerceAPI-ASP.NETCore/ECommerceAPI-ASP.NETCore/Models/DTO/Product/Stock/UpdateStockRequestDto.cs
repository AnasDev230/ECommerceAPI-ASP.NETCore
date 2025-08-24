namespace ECommerceAPI_ASP.NETCore.Models.DTO.Product.Stock
{
    public class UpdateStockRequestDto
    {
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
