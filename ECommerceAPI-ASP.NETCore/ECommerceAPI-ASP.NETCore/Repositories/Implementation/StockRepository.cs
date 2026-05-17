using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class StockRepository : IStockRepository
    {
        private readonly EcommerceDBContext dbContext;

        public StockRepository(EcommerceDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await dbContext.Stocks.AddAsync(stock);
            await dbContext.SaveChangesAsync();
            return stock;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var isInCart = await dbContext.ShoppingCartItems.AnyAsync(c => c.StockId == id);
            if (isInCart)
                throw new InvalidOperationException("Cannot delete stock that is in shopping carts.");

            var rowsAffected = await dbContext.Stocks
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Stock>> GetAllByProductIdAsync(Guid productId)
        {
            return await dbContext.Stocks
                .AsNoTracking()
                .Include(s => s.Product)
                .Include(s => s.Image)
                .Where(s => s.ProductId == productId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Stock>> GetAllStocksAsync()
        {
            return await dbContext.Stocks
                .AsNoTracking()
                .Include(s => s.Product)
                .Include(s => s.Image)
                .ToListAsync();
        }

        public async Task<Stock?> GetByID(Guid id)
        {
            return await dbContext.Stocks
                .AsNoTracking()
                .Include(s => s.Product)
                .Include(s => s.Image)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> IsUsedInCartAsync(Guid stockId)
        {
            return await dbContext.ShoppingCartItems.AnyAsync(c => c.StockId == stockId);
        }

        public async Task<bool> UpdateAsync(Stock stock)
        {
            var rowsAffected = await dbContext.Stocks
                .Where(s => s.Id == stock.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.SKU, stock.SKU)
                    .SetProperty(s => s.Color, stock.Color)
                    .SetProperty(s => s.Size, stock.Size)
                    .SetProperty(s => s.Quantity, stock.Quantity)
                    .SetProperty(s => s.Price, stock.Price)
                    .SetProperty(s => s.ImageID, stock.ImageID)
                    .SetProperty(s => s.ProductId, stock.ProductId)
                    .SetProperty(s => s.UpdatedAt, DateTime.UtcNow));

            return rowsAffected > 0;
        }
    }
}
