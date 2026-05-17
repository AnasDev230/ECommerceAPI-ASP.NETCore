using ECommerceAPI_ASP.NETCore.Models.DTO;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
var Image = new Models.Domain.Image
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = Path.GetFileNameWithoutExtension(file.FileName),
                    Title = file.FileName,
                    CreatedAt = DateTime.UtcNow,
                };
                Image = await imageRepository.UploadAsync(file, Image);
                ImageDto response = new ImageDto
                {
                    ID = Image.Id,
                    FileExtension = Image.FileExtension,
                    FileName = Image.FileName,
                    Title = Image.Title,
                    DateCreated = DateTime.UtcNow,
                    Url = Image.Url
                };
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var images = await imageRepository.GetAllAsync();
            var response = new List<ImageDto>();
            foreach (var Image in images)
            {
                response.Add(new ImageDto
                {
ID = Image.Id,
                    FileExtension = Image.FileExtension,
                    FileName = Image.FileName,
                    Title = Image.Title,
                    DateCreated = Image.CreatedAt,
                    Url = Image.Url
                });
            }
            return Ok(response);
        }

        [HttpDelete("{ID:guid}")]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> DeleteImage(Guid ID)
        {
            var image = await imageRepository.GetByID(ID);
            if (image == null)
                return NotFound();
            var deleted = await imageRepository.DeleteAsync(ID);
            if (!deleted)
                return NotFound();
            var response = new ImageDto
            {
                ID = image.Id,
                FileExtension = image.FileExtension,
                FileName = image.FileName,
                Title = image.Title,
                DateCreated = image.CreatedAt,
                Url = image.Url
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetImageByID([FromRoute] Guid id)
        {
            var image=await imageRepository.GetByID(id);
            if(image == null)
                return NotFound();
            var response = new ImageDto
            {
                ID = image.Id,
                FileExtension = image.FileExtension,
                FileName = image.FileName,
                Title = image.Title,
                DateCreated = image.CreatedAt,
                Url = image.Url
            };
            return Ok(response);
        }


        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpej", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported File Format");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size Cannot Be More Than 10 Mb");
            }
        }
    }
}
