using Blog_API.Models.DTO;
using Blog_API.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
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
                    DateCreated = DateTime.Now,
                };
                Image = await imageRepository.Upload(file, Image);
                ImageDto response = new ImageDto
                {
                    ID = Image.ID,
                    FileExtension = Image.FileExtension,
                    FileName = Image.FileName,
                    Title = Image.Title,
                    DateCreated = DateTime.Now,
                    Url = Image.Url
                };
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var images = await imageRepository.GetAll();
            var response = new List<ImageDto>();
            foreach (var Image in images)
            {
                response.Add(new ImageDto
                {
                    ID = Image.ID,
                    FileExtension = Image.FileExtension,
                    FileName = Image.FileName,
                    Title = Image.Title,
                    DateCreated = Image.DateCreated,
                    Url = Image.Url
                });
            }
            return Ok(response);
        }

        [HttpDelete("{ID:guid}")]
        public async Task<IActionResult> DeleteImage(Guid ID)
        {
            var image = await imageRepository.DeleteImage(ID);
            if (image == null)
                return NotFound();
            var response = new ImageDto
            {
                ID = image.ID,
                FileExtension = image.FileExtension,
                FileName = image.FileName,
                Title = image.Title,
                DateCreated = image.DateCreated,
                Url = image.Url
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageByID([FromRoute] Guid id)
        {
            var image=await imageRepository.GetByID(id);
            if(image == null)
                return NotFound();
            var response = new ImageDto
            {
                ID = image.ID,
                FileExtension = image.FileExtension,
                FileName = image.FileName,
                Title = image.Title,
                DateCreated = image.DateCreated,
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
