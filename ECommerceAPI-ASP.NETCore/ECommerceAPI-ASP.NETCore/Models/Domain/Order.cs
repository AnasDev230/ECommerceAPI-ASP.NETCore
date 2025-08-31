using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class Order
    {
        public Guid Id { get; set; }

        public string CustomerId { get; set; }
        public IdentityUser Customer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";
        // Pending, Paid, Shipped, Completed, Cancelled

        public ICollection<OrderItem> Items { get; set; }
    }
}
