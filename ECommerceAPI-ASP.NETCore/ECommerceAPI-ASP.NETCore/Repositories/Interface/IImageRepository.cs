using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<Image> UploadAsync(IFormFile file, Image image);
        Task<IEnumerable<Image>> GetAllAsync();
        Task<Image?> GetByID(Guid id);
        Task<Image?> DeleteAsync(Guid id);
    }
}
