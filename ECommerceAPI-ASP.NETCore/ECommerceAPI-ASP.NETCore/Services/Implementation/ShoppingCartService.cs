using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IMapper mapper;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IMapper mapper)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.mapper = mapper;
        }

        public async Task<ShoppingCartDto> CreateAsync(string customerId)
        {
            var existingCart = await shoppingCartRepository.GetCartByCustomerIdAsync(customerId);
            if (existingCart != null)
                throw new InvalidOperationException("Customer already has a shopping cart.");

            var shoppingCart = new ShoppingCart
            {
                CustomerId = customerId,
            };

            await shoppingCartRepository.CreateAsync(shoppingCart);
            return mapper.Map<ShoppingCartDto>(shoppingCart);
        }

        public async Task<ShoppingCartDto?> GetByCustomerIdAsync(string customerId)
        {
            var shoppingCart = await shoppingCartRepository.GetCartByCustomerIdAsync(customerId);
            if (shoppingCart == null)
                return null;

            return mapper.Map<ShoppingCartDto>(shoppingCart);
        }

        public async Task<ShoppingCartDto?> GetByIdAsync(Guid id)
        {
            var shoppingCart = await shoppingCartRepository.GetCartByID(id);
            if (shoppingCart == null)
                return null;

            return mapper.Map<ShoppingCartDto>(shoppingCart);
        }

        public async Task<ShoppingCartDto?> DeleteAsync(string customerId)
        {
            var shoppingCart = await shoppingCartRepository.GetCartByCustomerIdAsync(customerId);
            if (shoppingCart == null)
                return null;

            var deletedCart = await shoppingCartRepository.DeleteAsync(shoppingCart.Id);
            return mapper.Map<ShoppingCartDto>(deletedCart);
        }

        public async Task<ShoppingCartDto> GetOrCreateAsync(string customerId)
        {
            var cart = await shoppingCartRepository.GetOrCreateAsync(customerId);
            return mapper.Map<ShoppingCartDto>(cart);
        }
    }
}
