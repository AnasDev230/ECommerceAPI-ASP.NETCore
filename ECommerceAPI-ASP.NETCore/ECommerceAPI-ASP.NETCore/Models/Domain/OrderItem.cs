namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid StockId { get; set; }
        public Stock Stock { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
