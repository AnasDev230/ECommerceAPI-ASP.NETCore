using ECommerceAPI_ASP.NETCore.Models.DTO.Category;

namespace ECommerceAPI_ASP.NETCore.Services.Interface
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateAsync(CreateCategoryRequestDto request);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task<CategoryDto?> UpdateAsync(Guid id, CreateCategoryRequestDto request);
        Task<CategoryDto?> DeleteAsync(Guid id);
        Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync();
    }
}
