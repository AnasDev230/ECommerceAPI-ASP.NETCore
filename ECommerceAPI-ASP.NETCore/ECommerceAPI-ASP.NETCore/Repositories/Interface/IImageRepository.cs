using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<Image> Upload(IFormFile file, Image image);
        Task<IEnumerable<Image>> GetAll();
        Task<Image> GetByID(Guid id);
        Task<Image?> DeleteImage(Guid id);
    }
}
