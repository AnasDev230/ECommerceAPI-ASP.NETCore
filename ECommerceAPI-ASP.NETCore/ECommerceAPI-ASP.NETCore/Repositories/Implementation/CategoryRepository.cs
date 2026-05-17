using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EcommerceDBContext dbContext;

        public CategoryRepository(EcommerceDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories
                .AsNoTracking()
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Image)
                .ToListAsync();
        }

        public async Task<Category?> GetByID(Guid id)
        {
            return await dbContext.Categories
                .AsNoTracking()
                .AsSplitQuery()
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .Include(c => c.Image)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            var rowsAffected = await dbContext.Categories
                .Where(c => c.Id == category.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(c => c.Name, category.Name)
                    .SetProperty(c => c.Description, category.Description)
                    .SetProperty(c => c.DisplayOrder, category.DisplayOrder)
                    .SetProperty(c => c.IsActive, category.IsActive)
                    .SetProperty(c => c.ImageID, category.ImageID)
                    .SetProperty(c => c.ParentCategoryId, category.ParentCategoryId)
                    .SetProperty(c => c.UpdatedAt, DateTime.UtcNow));

            return rowsAffected > 0;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var category = await dbContext.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category is null)
                return null;

            if (category.SubCategories.Any())
                throw new InvalidOperationException("Cannot delete category with subcategories.");

            if (category.Products.Any())
                throw new InvalidOperationException("Cannot delete category with products.");

            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<bool> HasSubCategoriesAsync(Guid categoryId)
        {
            return await dbContext.Categories.AnyAsync(c => c.ParentCategoryId == categoryId);
        }

        public async Task<bool> HasProductsAsync(Guid categoryId)
        {
            return await dbContext.Products.AnyAsync(p => p.CategoryId == categoryId);
        }
    }
}
