using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart.ShoppingCartItem;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class ShoppingCartItemService : IShoppingCartItemService
    {
        private readonly IShoppingCartItemRepository shoppingCartItemRepository;
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IMapper mapper;

        public ShoppingCartItemService(IShoppingCartItemRepository shoppingCartItemRepository, IShoppingCartRepository shoppingCartRepository, IMapper mapper)
        {
            this.shoppingCartItemRepository = shoppingCartItemRepository;
            this.shoppingCartRepository = shoppingCartRepository;
            this.mapper = mapper;
        }

        public async Task<ShoppingCartItemDto> AddToCartAsync(string customerId, CreateShoppingCartItemRequestDto request)
        {
            var cart = await shoppingCartRepository.GetOrCreateAsync(customerId);

            var existingItems = await shoppingCartItemRepository.GetItemsByCartIdAsync(cart.Id);
            var existingItem = existingItems.FirstOrDefault(i => i.StockId == request.StockId);

            if (existingItem != null)
            {
                var newQuantity = existingItem.Quantity + request.Quantity;
                await shoppingCartItemRepository.UpdateQuantityAsync(existingItem.Id, newQuantity);
                var updatedItem = await shoppingCartItemRepository.GetByIdAsync(existingItem.Id);
                return mapper.Map<ShoppingCartItemDto>(updatedItem);
            }

            var item = new ShoppingCartItem
            {
                ShoppingCartId = cart.Id,
                StockId = request.StockId,
                Quantity = request.Quantity,
            };

            await shoppingCartItemRepository.CreateAsync(item);
            return mapper.Map<ShoppingCartItemDto>(item);
        }

        public async Task<ShoppingCartItemDto?> GetByIdAsync(Guid id)
        {
            var item = await shoppingCartItemRepository.GetByIdAsync(id);
            if (item == null)
                return null;

            return mapper.Map<ShoppingCartItemDto>(item);
        }

        public async Task<IEnumerable<ShoppingCartItemDto>> GetByCartIdAsync(Guid cartId)
        {
            var items = await shoppingCartItemRepository.GetItemsByCartIdAsync(cartId);
            return mapper.Map<IEnumerable<ShoppingCartItemDto>>(items);
        }

        public async Task<ShoppingCartItemDto?> UpdateQuantityAsync(Guid id, UpdateShoppingCartItemRequestDto request)
        {
            var item = await shoppingCartItemRepository.GetByIdAsync(id);
            if (item == null)
                return null;

            var updated = await shoppingCartItemRepository.UpdateQuantityAsync(id, request.Quantity);
            if (!updated)
                return null;

            var updatedItem = await shoppingCartItemRepository.GetByIdAsync(id);
            return mapper.Map<ShoppingCartItemDto>(updatedItem);
        }

        public async Task<ShoppingCartItemDto?> DeleteAsync(Guid id)
        {
            var item = await shoppingCartItemRepository.GetByIdAsync(id);
            if (item == null)
                return null;

            var deleted = await shoppingCartItemRepository.DeleteAsync(id);
            if (!deleted)
                return null;

            return mapper.Map<ShoppingCartItemDto>(item);
        }
    }
}
