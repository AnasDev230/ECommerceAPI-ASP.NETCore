using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Category;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryRequestDto request)
        {
            if (request.ParentCategoryId.HasValue)
            {
                var parentCategory = await categoryRepository.GetByID(request.ParentCategoryId.Value);
                if (parentCategory == null)
                    throw new KeyNotFoundException($"Parent category with ID {request.ParentCategoryId.Value} not found.");
            }

            var category = mapper.Map<Category>(request);
            await categoryRepository.CreateAsync(category);
            return mapper.Map<CategoryDto>(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await categoryRepository.GetAllAsync();
            return mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var category = await categoryRepository.GetByID(id);
            if (category == null)
                return null;

            return mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto?> UpdateAsync(Guid id, CreateCategoryRequestDto request)
        {
            var existingCategory = await categoryRepository.GetByID(id);
            if (existingCategory == null)
                return null;

            if (request.ParentCategoryId.HasValue)
            {
                if (request.ParentCategoryId.Value == id)
                    throw new InvalidOperationException("A category cannot be its own parent.");

                var parentCategory = await categoryRepository.GetByID(request.ParentCategoryId.Value);
                if (parentCategory == null)
                    throw new KeyNotFoundException($"Parent category with ID {request.ParentCategoryId.Value} not found.");
            }

            mapper.Map(request, existingCategory);
            var updated = await categoryRepository.UpdateAsync(existingCategory);
            if (!updated)
                return null;

            var updatedCategory = await categoryRepository.GetByID(id);
            return mapper.Map<CategoryDto>(updatedCategory);
        }

        public async Task<CategoryDto?> DeleteAsync(Guid id)
        {
            var category = await categoryRepository.GetByID(id);
            if (category == null)
                return null;

            var deletedCategory = await categoryRepository.DeleteAsync(id);
            return mapper.Map<CategoryDto>(deletedCategory);
        }

        public async Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync()
        {
            var categories = await categoryRepository.GetAllAsync();
            var activeCategories = categories.Where(c => c.IsActive);
            return mapper.Map<IEnumerable<CategoryDto>>(activeCategories);
        }
    }
}
