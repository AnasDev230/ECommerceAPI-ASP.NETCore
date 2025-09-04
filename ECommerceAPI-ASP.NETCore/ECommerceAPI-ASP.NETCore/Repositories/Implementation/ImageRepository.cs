using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EcommerceDBContext dBContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor, EcommerceDBContext dBContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dBContext = dBContext;
        }

        public async Task<Image> DeleteImage(Guid id)
        {
            var image = await dBContext.Images.FindAsync(id);
            if (image == null)
            {
                return null;
            }
            dBContext.Images.Remove(image);
            await dBContext.SaveChangesAsync();
            return image;
        }

        public async Task<IEnumerable<Image>> GetAll()
        {
            return await dBContext.Images.ToListAsync();
        }

        public async Task<Image> GetByID(Guid id)
        {
            return await dBContext.Images.FindAsync(id);
        }

        public async Task<Image> Upload(IFormFile file, Image image)
        {
            var imagesFolder = Path.Combine(webHostEnvironment.ContentRootPath, "Images");
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);


            var guidFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";



            var localPath = Path.Combine(imagesFolder,guidFileName);
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{guidFileName}";
            image.Url = urlPath;
            await dBContext.Images.AddAsync(image);
            await dBContext.SaveChangesAsync();
            return image;
        }

    }
}
