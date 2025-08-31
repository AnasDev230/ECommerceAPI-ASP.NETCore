using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class ShoppingCartItemRepository : IShoppingCartItemRepository
    {
        private readonly EcommerceDBContext dBcontext;
        public ShoppingCartItemRepository(EcommerceDBContext dBContext)
        {
            this.dBcontext = dBContext;
        }
        public async Task<ShoppingCartItem> CreateAsync(ShoppingCartItem item)
        {
            //var stock = await dBcontext.Stocks.FirstOrDefaultAsync(x=>x.Id==item.StockId);
            //if (stock == null)
            //    throw new Exception("Stock Not Found.");
            //if (stock.Quantity < item.Quantity)
            //    throw new Exception("Not enough stock available.");

            //stock.Quantity -= item.Quantity;

            await dBcontext.ShoppingCartItems.AddAsync(item);
            await dBcontext.SaveChangesAsync(); 
            return item;
        }

        public async Task<ShoppingCartItem> DeleteAsync(Guid id)
        {
            var shoppingCartItem=await dBcontext.ShoppingCartItems.FindAsync(id);
            if (shoppingCartItem != null) { 
                dBcontext.ShoppingCartItems.Remove(shoppingCartItem);
                await dBcontext.SaveChangesAsync();
                return shoppingCartItem;
            }
            return null;
        }

        public async Task<ShoppingCartItem?> GetByIdAsync(Guid id)
        {
            return await dBcontext.ShoppingCartItems.FindAsync(id);
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetItemsByCartIdAsync(Guid cartId)
        {
            return await dBcontext.ShoppingCartItems.Where(x => x.ShoppingCartId == cartId).ToListAsync();
        }

        public async Task<ShoppingCartItem?> UpdateQuantityAsync(Guid id, int quantity)
        {
            var shoppingCartItem = await dBcontext.ShoppingCartItems.FindAsync(id);
            if (shoppingCartItem == null)
                return null;
            shoppingCartItem.Quantity = quantity;
            await dBcontext.SaveChangesAsync();
            return shoppingCartItem;
        }
    }
}
