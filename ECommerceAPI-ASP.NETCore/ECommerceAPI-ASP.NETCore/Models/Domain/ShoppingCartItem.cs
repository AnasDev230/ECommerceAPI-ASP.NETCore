using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class ShoppingCartItem : BaseEntity
    {
        [Required]
        public Guid ShoppingCartId { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }

        [Required]
        public Guid StockId { get; set; }
        public Stock? Stock { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
