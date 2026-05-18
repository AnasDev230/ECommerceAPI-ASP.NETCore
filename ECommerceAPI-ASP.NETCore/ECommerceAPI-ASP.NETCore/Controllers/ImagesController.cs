using ECommerceAPI_ASP.NETCore.Models.DTO.Image;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService imageService;

        public ImagesController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [HttpPost(Name = "UploadImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] string? title = null, [FromForm] string? altText = null)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file was provided.");

            var image = await imageService.UploadAsync(file, title, altText);
            return Ok(image);
        }

        [HttpGet(Name = "GetAllImages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var images = await imageService.GetAllAsync();
            return Ok(images);
        }

        [HttpGet("{id}", Name = "GetImageByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetImageByID([FromRoute] Guid id)
        {
            var image = await imageService.GetByIdAsync(id);
            if (image == null)
                return NotFound();
            return Ok(image);
        }

        [HttpDelete("{id}", Name = "DeleteImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> DeleteImage([FromRoute] Guid id)
        {
            var image = await imageService.DeleteAsync(id);
            if (image == null)
                return NotFound();
            return Ok(image);
        }
    }
}
