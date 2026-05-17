using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Category;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoriesController(ICategoryRepository categoryRepository,IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }
        [HttpPost("Add", Name = "AddCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryRequestDto request)
        {
            var category = mapper.Map<Category>(request);
            await categoryRepository.CreateAsync(category);
            return Created("", mapper.Map<CategoryDto>(category));
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();
            return Ok(mapper.Map<List<CategoryDto>>(categories));
        }
        [HttpGet("GetByID/{ID}", Name = "GetCategoryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetCategoryByID([FromRoute] Guid ID)
        {
            Category category = await categoryRepository.GetByID(ID);
           if(category == null)
                return NotFound();
           return Ok(mapper.Map<CategoryDto>(category));
        }

        [HttpPut("{ID:Guid}", Name = "EditCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid ID, CreateCategoryRequestDto updateCategoryRequestDto)
        {
            var category= await categoryRepository.GetByID(ID);
            if(category==null)
                return NotFound();
            mapper.Map(updateCategoryRequestDto, category);
            var updated = await categoryRepository.UpdateAsync(category);
            if (!updated)
                return NotFound();
            var updatedCategory = await categoryRepository.GetByID(ID);
            return Ok(mapper.Map<CategoryDto>(updatedCategory));
        }

        [HttpDelete]
        [Route("{ID:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid ID)
        {
            var category = await categoryRepository.GetByID(ID);
            if (category == null)
                return NotFound();
            category=await categoryRepository.DeleteAsync(ID);
            return Ok(mapper.Map<CategoryDto>(category));
        }
    }
}
