using ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart.ShoppingCartItem;

namespace ECommerceAPI_ASP.NETCore.Services.Interface
{
    public interface IShoppingCartItemService
    {
        Task<ShoppingCartItemDto> AddToCartAsync(string customerId, CreateShoppingCartItemRequestDto request);
        Task<ShoppingCartItemDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ShoppingCartItemDto>> GetByCartIdAsync(Guid cartId);
        Task<ShoppingCartItemDto?> UpdateQuantityAsync(Guid id, UpdateShoppingCartItemRequestDto request);
        Task<ShoppingCartItemDto?> DeleteAsync(Guid id);
    }
}
