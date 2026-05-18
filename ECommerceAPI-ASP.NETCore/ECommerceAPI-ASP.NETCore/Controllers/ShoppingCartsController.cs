using System.Security.Claims;
using ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartsController(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        [HttpPost("Add", Name = "AddShoppingCart")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddShoppingCart()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            try
            {
                var shoppingCart = await shoppingCartService.CreateAsync(customerId);
                return Created("", shoppingCart);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetByCustomerID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCartByCustomerIdAsync()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            var shoppingCart = await shoppingCartService.GetByCustomerIdAsync(customerId);
            if (shoppingCart == null)
                return NotFound();
            return Ok(shoppingCart);
        }

        [HttpGet("{shoppingCartID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetCartByID([FromRoute] Guid shoppingCartID)
        {
            var shoppingCart = await shoppingCartService.GetByIdAsync(shoppingCartID);
            if (shoppingCart == null)
                return NotFound();
            return Ok(shoppingCart);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteShoppingCart()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            var shoppingCart = await shoppingCartService.DeleteAsync(customerId);
            if (shoppingCart == null)
                return NotFound();
            return Ok(shoppingCart);
        }
    }
}
