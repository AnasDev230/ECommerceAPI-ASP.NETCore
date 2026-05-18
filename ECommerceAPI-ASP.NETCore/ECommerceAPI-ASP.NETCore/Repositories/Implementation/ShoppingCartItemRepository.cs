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

        public async Task<bool> DeleteAsync(Guid id)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var item = await dbContext.ShoppingCartItems
                    .Include(x => x.Stock)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (item == null)
                    return false;

                if (item.Stock != null)
                    item.Stock.Quantity += item.Quantity;

                var rowsAffected = await dbContext.ShoppingCartItems
                    .Where(i => i.Id == id)
                    .ExecuteDeleteAsync();

                await transaction.CommitAsync();
                return rowsAffected > 0;
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
                .AsNoTracking()
                .Include(x => x.Stock!)
                .ThenInclude(s => s.Product!)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetItemsByCartIdAsync(Guid cartId)
        {
            return await dbContext.ShoppingCartItems
                .AsNoTracking()
                .Include(x => x.Stock!)
                .ThenInclude(s => s.Product!)
                .Where(x => x.ShoppingCartId == cartId)
                .ToListAsync();
        }

        public async Task<bool> UpdateQuantityAsync(Guid id, int quantity)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                if (quantity <= 0)
                    throw new InvalidOperationException("Quantity must be greater than zero.");

                var item = await dbContext.ShoppingCartItems
                    .Include(x => x.Stock)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (item == null)
                    return false;

                var quantityDifference = quantity - item.Quantity;

                if (quantityDifference > 0)
                {
                    if (item.Stock == null)
                        throw new InvalidOperationException("Stock not found for this item.");
                    if (item.Stock.Quantity < quantityDifference)
                        throw new InvalidOperationException("Not enough stock available.");

                    var stockId = item.StockId;
                    var newQuantity = item.Stock.Quantity - quantityDifference;
                    await dbContext.Stocks
                        .Where(s => s.Id == stockId)
                        .ExecuteUpdateAsync(setters => setters.SetProperty(s => s.Quantity, newQuantity));
                }
                else if (quantityDifference < 0)
                {
                    var stockId = item.StockId;
                    var currentStock = await dbContext.Stocks.FindAsync(stockId);
                    var newQuantity = currentStock!.Quantity + Math.Abs(quantityDifference);
                    await dbContext.Stocks
                        .Where(s => s.Id == stockId)
                        .ExecuteUpdateAsync(setters => setters.SetProperty(s => s.Quantity, newQuantity));
                }

                await dbContext.ShoppingCartItems
                    .Where(i => i.Id == id)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.Quantity, quantity));

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
