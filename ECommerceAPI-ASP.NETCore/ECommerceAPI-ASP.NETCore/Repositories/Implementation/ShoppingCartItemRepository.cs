using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class ShoppingCartItemRepository : IShoppingCartItemRepository
    {
        private readonly EcommerceDBContext dbContext;

        public ShoppingCartItemRepository(EcommerceDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ShoppingCartItem> CreateAsync(ShoppingCartItem item)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var stock = await dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == item.StockId);
                if (stock == null)
                    throw new InvalidOperationException("Stock not found.");
                if (stock.Quantity < item.Quantity)
                    throw new InvalidOperationException("Not enough stock available.");

                stock.Quantity -= item.Quantity;

                await dbContext.ShoppingCartItems.AddAsync(item);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return item;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ShoppingCartItem?> DeleteAsync(Guid id)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var shoppingCartItem = await dbContext.ShoppingCartItems
                    .Include(x => x.Stock)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (shoppingCartItem == null)
                    return null;

                if (shoppingCartItem.Stock != null)
                    shoppingCartItem.Stock.Quantity += shoppingCartItem.Quantity;

                dbContext.ShoppingCartItems.Remove(shoppingCartItem);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return shoppingCartItem;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ShoppingCartItem?> GetByIdAsync(Guid id)
        {
            return await dbContext.ShoppingCartItems
                .Include(x => x.Stock)
                .ThenInclude(s => s.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetItemsByCartIdAsync(Guid cartId)
        {
            return await dbContext.ShoppingCartItems
                .Where(x => x.ShoppingCartId == cartId)
                .Include(x => x.Stock)
                .ThenInclude(s => s.Product)
                .ToListAsync();
        }

        public async Task<ShoppingCartItem?> UpdateQuantityAsync(Guid id, int quantity)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var shoppingCartItem = await dbContext.ShoppingCartItems
                    .Include(x => x.Stock)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (shoppingCartItem == null)
                    return null;

                if (quantity <= 0)
                    throw new InvalidOperationException("Quantity must be greater than zero.");

                var quantityDifference = quantity - shoppingCartItem.Quantity;

                if (quantityDifference > 0)
                {
                    if (shoppingCartItem.Stock == null)
                        throw new InvalidOperationException("Stock not found for this item.");
                    if (shoppingCartItem.Stock.Quantity < quantityDifference)
                        throw new InvalidOperationException("Not enough stock available.");

                    shoppingCartItem.Stock.Quantity -= quantityDifference;
                }
                else if (quantityDifference < 0)
                {
                    if (shoppingCartItem.Stock != null)
                        shoppingCartItem.Stock.Quantity += Math.Abs(quantityDifference);
                }

                shoppingCartItem.Quantity = quantity;
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return shoppingCartItem;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
