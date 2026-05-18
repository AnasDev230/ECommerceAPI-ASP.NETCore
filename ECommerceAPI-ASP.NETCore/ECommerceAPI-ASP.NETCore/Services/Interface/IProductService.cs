using ECommerceAPI_ASP.NETCore.Models.DTO.Product;

namespace ECommerceAPI_ASP.NETCore.Services.Interface
{
    public interface IProductService
    {
        Task<ProductDto> CreateAsync(CreateProductRequestDto request, string vendorId);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<IEnumerable<ProductDto>> GetAllActiveAsync();
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<ProductDto>> GetByVendorIdAsync(string vendorId);
        Task<ProductDto?> UpdateAsync(Guid id, CreateProductRequestDto request, string vendorId, bool isAdmin);
        Task<ProductDto?> DeleteAsync(Guid id, string vendorId, bool isAdmin);
    }
}
