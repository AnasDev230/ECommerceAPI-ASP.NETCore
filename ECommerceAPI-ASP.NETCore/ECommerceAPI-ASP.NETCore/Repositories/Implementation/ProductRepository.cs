using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceDBContext dBContext;

        public ProductRepository(EcommerceDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Product> CreateAsync(Product product)
        {
           await dBContext.Products.AddAsync(product);
            await dBContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(Guid id)
        {
            Product product = await dBContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
            {
                return null;
            }
            dBContext.Products.Remove(product);
            await dBContext.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProductsByCategoryID(Guid ID)
        {
            return await dBContext.Products.Where(p => p.CategoryId == ID).ToListAsync();
        }

        public Task<IEnumerable<Stock>> GetAllStockAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> GetByID(Guid id)
        {
            return await dBContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            Product existingProduct = await dBContext.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
            if (existingProduct != null)
            {
                dBContext.Entry(existingProduct).CurrentValues.SetValues(product);
                await dBContext.SaveChangesAsync();
                return product;
            }
            return null;
        }
    }
}
