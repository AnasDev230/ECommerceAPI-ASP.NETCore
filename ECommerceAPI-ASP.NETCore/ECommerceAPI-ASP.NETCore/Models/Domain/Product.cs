using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string VendorId { get; set; }
        public IdentityUser Vendor { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Stock> Stocks { get; set; }
    }
}
