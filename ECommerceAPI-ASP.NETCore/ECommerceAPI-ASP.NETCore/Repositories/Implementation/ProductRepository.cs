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
                .Include(p => p.Category)
                .Include(p => p.Image)
                .Include(p => p.Stocks)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsByCategoryID(Guid categoryId)
        {
            return await dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Image)
                .Include(p => p.Stocks)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<Product?> GetByID(Guid id)
        {
            return await dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Image)
                .Include(p => p.Stocks)
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existingProduct = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
            if (existingProduct == null)
                return null;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.DescriptionPlainText = product.DescriptionPlainText;
            existingProduct.SKU = product.SKU;
            existingProduct.Brand = product.Brand;
            existingProduct.BasePrice = product.BasePrice;
            existingProduct.SalePrice = product.SalePrice;
            existingProduct.IsActive = product.IsActive;
            existingProduct.Weight = product.Weight;
            existingProduct.ImageID = product.ImageID;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return existingProduct;
        }
    }
}
