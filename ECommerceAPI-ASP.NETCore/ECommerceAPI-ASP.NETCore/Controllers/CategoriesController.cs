using ECommerceAPI_ASP.NETCore.Models.DTO.Category;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpPost("Add", Name = "AddCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryRequestDto request)
        {
            var category = await categoryService.CreateAsync(request);
            return Created("", category);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("GetByID/{ID}", Name = "GetCategoryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryByID([FromRoute] Guid ID)
        {
            var category = await categoryService.GetByIdAsync(ID);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPut("{ID:Guid}", Name = "EditCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid ID, [FromBody] CreateCategoryRequestDto request)
        {
            var category = await categoryService.UpdateAsync(ID, request);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpDelete("{ID:Guid}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid ID)
        {
            var category = await categoryService.DeleteAsync(ID);
            if (category == null)
                return NotFound();
            return Ok(category);
        }
    }
}
