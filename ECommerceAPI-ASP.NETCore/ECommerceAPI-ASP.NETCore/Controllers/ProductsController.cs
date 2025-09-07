using System.Security.Claims;
using AutoMapper;
using Blog_API.Repositories.Implementation;
using Blog_API.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Category;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly ICategoryRepository categoryRepository;

        public ProductsController(IProductRepository productRepository, IMapper mapper,ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }
        [HttpPost("Add", Name = "AddProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductRequestDto request)
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (vendorId == null)
                return Unauthorized();
            var product = new Product
            {

                Name = request.Name,
                Description = request.Description,
                ImageID=request.ImageID,
                VendorId = vendorId,
                CategoryId = request.CategoryId,
            };
            await productRepository.CreateAsync(product);
            return Created("", mapper.Map<ProductDto>(product));
        }
        [HttpGet("GetByID/{ID}", Name = "GetProductByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetProductByID([FromRoute] Guid ID)
        {
            Product product=await productRepository.GetByID(ID);
            if(product == null)
                return NotFound();
            return Ok(mapper.Map<ProductDto>(product));
        }
        [HttpGet("GetProductsByCategoryID/{ID}", Name = "GetProductByCategoryID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetProductByCategoryID([FromRoute] Guid ID)
        {
            var category= await categoryRepository.GetByID(ID);
            if(category == null)
                return NotFound();
            var products= await productRepository.GetAllProductsByCategoryID(ID);
            return Ok(mapper.Map<List<ProductDto>>(products));
        }

        [HttpPut("{ID:Guid}", Name = "EditProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> EditProduct([FromRoute] Guid ID, CreateProductRequestDto updateProductRequestDto)
        {
            var product = await productRepository.GetByID(ID);
            if (product == null)
                return NotFound();
            mapper.Map(updateProductRequestDto, product);
            product = await productRepository.UpdateAsync(product);
            return Ok(mapper.Map<ProductDto>(product));
        }
        [HttpDelete]
        [Route("{ID:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid ID)
        {
            var product = await productRepository.GetByID(ID);
            if (product == null)
                return NotFound();
            product = await productRepository.DeleteAsync(ID);
            return Ok(mapper.Map<ProductDto>(product));
        }

    }
}
