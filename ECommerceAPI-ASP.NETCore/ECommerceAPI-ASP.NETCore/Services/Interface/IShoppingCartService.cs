using ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart;

namespace ECommerceAPI_ASP.NETCore.Services.Interface
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartDto> CreateAsync(string customerId);
        Task<ShoppingCartDto?> GetByCustomerIdAsync(string customerId);
        Task<ShoppingCartDto?> GetByIdAsync(Guid id);
        Task<ShoppingCartDto?> DeleteAsync(string customerId);
        Task<ShoppingCartDto> GetOrCreateAsync(string customerId);
    }
}
