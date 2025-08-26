using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> CreateAsync(ShoppingCart shoppingCart);
        Task<ShoppingCart?> GetCartByCustomerIdAsync(string customerId);
        Task<ShoppingCart?> GetCartByID(Guid id);
        Task<ShoppingCart?> DeleteAsync(Guid id);
    }
}
