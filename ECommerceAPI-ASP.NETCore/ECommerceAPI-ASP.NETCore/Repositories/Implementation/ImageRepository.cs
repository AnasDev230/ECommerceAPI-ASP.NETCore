using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EcommerceDBContext dbContext;

        private static readonly HashSet<string> AllowedExtensions = new() { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor, EcommerceDBContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var image = await dbContext.Images.FindAsync(id);
            if (image == null)
                return false;

            var filePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", image.FileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            var rowsAffected = await dbContext.Images
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Image>> GetAllAsync()
        {
            return await dbContext.Images
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Image?> GetByID(Guid id)
        {
            return await dbContext.Images
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Image> UploadAsync(IFormFile file, Image image)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new InvalidOperationException($"File type not allowed. Allowed types: {string.Join(", ", AllowedExtensions)}");

            if (file.Length > MaxFileSize)
                throw new InvalidOperationException($"File size exceeds maximum allowed size of {MaxFileSize / 1024 / 1024}MB.");

            var imagesFolder = Path.Combine(webHostEnvironment.ContentRootPath, "Images");
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            var guidFileName = $"{Guid.NewGuid()}{extension}";
            var localPath = Path.Combine(imagesFolder, guidFileName);

            await using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            image.FileName = guidFileName;
            image.FileExtension = extension;

            var httpRequest = httpContextAccessor.HttpContext?.Request;
            if (httpRequest != null)
                image.Url = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{guidFileName}";
            else
                image.Url = $"/Images/{guidFileName}";

            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;
        }
    }
}
