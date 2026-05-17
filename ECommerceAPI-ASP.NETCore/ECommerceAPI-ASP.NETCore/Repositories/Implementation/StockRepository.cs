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

        public async Task<Stock?> DeleteAsync(Guid id)
        {
            var stock = await dbContext.Stocks
                .Include(s => s.Product)
                .Include(s => s.Image)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stock == null)
                return null;

            var isInCart = await dbContext.ShoppingCartItems.AnyAsync(c => c.StockId == stock.Id);
            if (isInCart)
                throw new InvalidOperationException("Cannot delete stock that is in shopping carts.");

            dbContext.Stocks.Remove(stock);
            await dbContext.SaveChangesAsync();
            return stock;
        }

        public async Task<IEnumerable<Stock>> GetAllByProductIdAsync(Guid productId)
        {
            return await dbContext.Stocks
                .Where(x => x.ProductId == productId)
                .Include(x => x.Product)
                .Include(x => x.Image)
                .ToListAsync();
        }

        public async Task<IEnumerable<Stock>> GetAllStocksAsync()
        {
            return await dbContext.Stocks
                .Include(s => s.Product)
                .Include(s => s.Image)
                .ToListAsync();
        }

        public async Task<Stock?> GetByID(Guid id)
        {
            return await dbContext.Stocks
                .Include(s => s.Product)
                .Include(s => s.Image)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsUsedInCartAsync(Guid stockId)
        {
            return await dbContext.ShoppingCartItems.AnyAsync(c => c.StockId == stockId);
        }

        public async Task<Stock?> UpdateAsync(Stock stock)
        {
            var existingStock = await dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == stock.Id);
            if (existingStock == null)
                return null;

            existingStock.SKU = stock.SKU;
            existingStock.Color = stock.Color;
            existingStock.Size = stock.Size;
            existingStock.Quantity = stock.Quantity;
            existingStock.Price = stock.Price;
            existingStock.ImageID = stock.ImageID;
            existingStock.ProductId = stock.ProductId;
            existingStock.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return existingStock;
        }
    }
}
