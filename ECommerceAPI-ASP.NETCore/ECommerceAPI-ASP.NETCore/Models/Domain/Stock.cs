namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class Stock
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    }
}
