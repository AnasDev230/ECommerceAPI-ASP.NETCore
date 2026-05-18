using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Image;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository imageRepository;
        private readonly IMapper mapper;

        public ImageService(IImageRepository imageRepository, IMapper mapper)
        {
            this.imageRepository = imageRepository;
            this.mapper = mapper;
        }

        public async Task<ImageDto> UploadAsync(IFormFile file, string? title = null, string? altText = null)
        {
            var image = new Image
            {
                Title = title ?? Path.GetFileNameWithoutExtension(file.FileName),
                AltText = altText,
                IsPrimary = false,
                DisplayOrder = 0,
            };

            var uploadedImage = await imageRepository.UploadAsync(file, image);
            return mapper.Map<ImageDto>(uploadedImage);
        }

        public async Task<IEnumerable<ImageDto>> GetAllAsync()
        {
            var images = await imageRepository.GetAllAsync();
            return mapper.Map<IEnumerable<ImageDto>>(images);
        }

        public async Task<ImageDto?> GetByIdAsync(Guid id)
        {
            var image = await imageRepository.GetByID(id);
            if (image == null)
                return null;

            return mapper.Map<ImageDto>(image);
        }

        public async Task<ImageDto?> DeleteAsync(Guid id)
        {
            var image = await imageRepository.GetByID(id);
            if (image == null)
                return null;

            var deleted = await imageRepository.DeleteAsync(id);
            if (!deleted)
                return null;

            return mapper.Map<ImageDto>(image);
        }
    }
}
