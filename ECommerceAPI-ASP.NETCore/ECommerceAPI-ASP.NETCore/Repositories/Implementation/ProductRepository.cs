using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceDBContext dbContext;

        public ProductRepository(EcommerceDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(Guid id)
        {
            var product = await dbContext.Products
                .Include(p => p.Ratings)
                .Include(p => p.Stocks)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
                return null;

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Image)
                .Include(p => p.Stocks)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsByCategoryID(Guid categoryId)
        {
            return await dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Image)
                .Include(p => p.Stocks)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<Product?> GetByID(Guid id)
        {
            return await dbContext.Products
                .AsNoTracking()
                .AsSplitQuery()
                .Include(p => p.Category)
                .Include(p => p.Image)
                .Include(p => p.Stocks)
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var rowsAffected = await dbContext.Products
                .Where(p => p.Id == product.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.Name, product.Name)
                    .SetProperty(p => p.Description, product.Description)
                    .SetProperty(p => p.DescriptionPlainText, product.DescriptionPlainText)
                    .SetProperty(p => p.Brand, product.Brand)
                    .SetProperty(p => p.IsActive, product.IsActive)
                    .SetProperty(p => p.Weight, product.Weight)
                    .SetProperty(p => p.ImageID, product.ImageID)
                    .SetProperty(p => p.CategoryId, product.CategoryId)
                    .SetProperty(p => p.UpdatedAt, DateTime.UtcNow));

            return rowsAffected > 0;
        }
    }
}
