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
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Image)
                .ToListAsync();
        }

        public async Task<Category?> GetByID(Guid id)
        {
            return await dbContext.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .Include(c => c.Image)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (existingCategory == null)
                return null;

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            existingCategory.DisplayOrder = category.DisplayOrder;
            existingCategory.IsActive = category.IsActive;
            existingCategory.ImageID = category.ImageID;
            existingCategory.ParentCategoryId = category.ParentCategoryId;
            existingCategory.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return existingCategory;
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
