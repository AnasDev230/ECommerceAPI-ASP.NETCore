using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly EcommerceDBContext dbContext;

        public ShoppingCartRepository(EcommerceDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ShoppingCart> CreateAsync(ShoppingCart shoppingCart)
        {
            await dbContext.ShoppingCarts.AddAsync(shoppingCart);
            await dbContext.SaveChangesAsync();
            return shoppingCart;
        }

        public async Task<ShoppingCart?> DeleteAsync(Guid id)
        {
            var shoppingCart = await dbContext.ShoppingCarts
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shoppingCart == null)
                return null;

            dbContext.ShoppingCarts.Remove(shoppingCart);
            await dbContext.SaveChangesAsync();
            return shoppingCart;
        }

        public async Task<ShoppingCart?> GetCartByCustomerIdAsync(string customerId)
        {
            return await dbContext.ShoppingCarts
                .AsNoTracking()
                .Include(x => x.Items)
                .ThenInclude(i => i.Stock)
                .ThenInclude(s => s.Product)
                .FirstOrDefaultAsync(x => x.CustomerId == customerId);
        }

        public async Task<ShoppingCart?> GetCartByID(Guid id)
        {
            return await dbContext.ShoppingCarts
                .AsNoTracking()
                .Include(x => x.Items)
                .ThenInclude(i => i.Stock)
                .ThenInclude(s => s.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ShoppingCart> GetOrCreateAsync(string customerId)
        {
            var cart = await dbContext.ShoppingCarts
                .AsNoTracking()
                .Include(x => x.Items)
                .ThenInclude(i => i.Stock)
                .ThenInclude(s => s.Product)
                .FirstOrDefaultAsync(x => x.CustomerId == customerId);

            if (cart != null)
                return cart;

            cart = new ShoppingCart
            {
                CustomerId = customerId,
                Items = new List<ShoppingCartItem>()
            };

            await dbContext.ShoppingCarts.AddAsync(cart);
            await dbContext.SaveChangesAsync();

            return cart;
        }
    }
}
