using ECommerceAPI_ASP.NETCore.Models.DTO.Image;
using Microsoft.AspNetCore.Http;

namespace ECommerceAPI_ASP.NETCore.Services.Interface
{
    public interface IImageService
    {
        Task<ImageDto> UploadAsync(IFormFile file, string? title = null, string? altText = null);
        Task<IEnumerable<ImageDto>> GetAllAsync();
        Task<ImageDto?> GetByIdAsync(Guid id);
        Task<ImageDto?> DeleteAsync(Guid id);
    }
}
