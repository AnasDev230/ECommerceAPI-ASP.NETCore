using System.Security.Claims;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost("Add", Name = "AddProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductRequestDto request)
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (vendorId == null)
                return Unauthorized();

            var product = await productService.CreateAsync(request, vendorId);
            return CreatedAtAction(nameof(GetProductByID), new { ID = product.Id }, product);
        }

        [HttpGet("GetByID/{ID}", Name = "GetProductByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetProductByID([FromRoute] Guid ID)
        {
            var product = await productService.GetByIdAsync(ID);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpGet("GetProductsByCategoryID/{ID}", Name = "GetProductByCategoryID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetProductByCategoryID([FromRoute] Guid ID)
        {
            var products = await productService.GetByCategoryIdAsync(ID);
            return Ok(products);
        }

        [HttpPut("{ID:Guid}", Name = "EditProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> EditProduct([FromRoute] Guid ID, [FromBody] CreateProductRequestDto request)
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (vendorId == null)
                return Unauthorized();

            var isAdmin = User.IsInRole("Admin");

            var product = await productService.UpdateAsync(ID, request, vendorId, isAdmin);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpDelete("{ID:Guid}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid ID)
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (vendorId == null)
                return Unauthorized();

            var isAdmin = User.IsInRole("Admin");

            var product = await productService.DeleteAsync(ID, vendorId, isAdmin);
            if (product == null)
                return NotFound();
            return Ok(product);
        }
    }
}
