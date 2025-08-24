using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class StockRepository : IStockRepository
    {
        private readonly EcommerceDBContext dBContext;
        public StockRepository(EcommerceDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Stock> CreateAsync(Stock stock)
        {
            await dBContext.Stocks.AddAsync(stock);
            await dBContext.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> DeleteAsync(Guid id)
        {
            var stock=dBContext.Stocks.FirstOrDefault(x => x.Id == id);
            if (stock == null)
                return null;
             dBContext.Stocks.Remove(stock);
            await dBContext.SaveChangesAsync();
            return stock;
        }

        public async Task<IEnumerable<Stock>> GetAllByProductIdAsync(Guid id)
        {
            return await dBContext.Stocks.Where(x => x.ProductId == id).Include(x=>x.Product).ToListAsync();
        }

        public async Task<Stock?> GetByID(Guid id)
        {
            return await dBContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateAsync(Stock stock)
        {
            Stock existingstock = await dBContext.Stocks.FirstOrDefaultAsync(x => x.Id == stock.Id);
            if (existingstock != null)
            {
                dBContext.Entry(existingstock).CurrentValues.SetValues(stock);
                await dBContext.SaveChangesAsync();
                return stock;
            }
            return null;
        }
    }
}
