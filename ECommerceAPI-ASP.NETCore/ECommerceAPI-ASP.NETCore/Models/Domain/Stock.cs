namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class Stock
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid? ImageID { get; set; }
        public Image Image { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    }
}
