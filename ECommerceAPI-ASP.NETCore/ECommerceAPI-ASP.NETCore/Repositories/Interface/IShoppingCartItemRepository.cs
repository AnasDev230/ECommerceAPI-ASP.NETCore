using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface IShoppingCartItemRepository
    {
        Task<ShoppingCartItem> CreateAsync(ShoppingCartItem item);
        Task<ShoppingCartItem?> GetByIdAsync(Guid id);
        Task<IEnumerable<ShoppingCartItem>> GetItemsByCartIdAsync(Guid cartId);
        Task<bool> UpdateQuantityAsync(Guid id, int quantity);
        Task<bool> DeleteAsync(Guid id);
    }
}
