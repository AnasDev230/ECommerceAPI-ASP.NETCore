using System.Security.Claims;
using ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart.ShoppingCartItem;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartItemsController : ControllerBase
    {
        private readonly IShoppingCartItemService shoppingCartItemService;

        public ShoppingCartItemsController(IShoppingCartItemService shoppingCartItemService)
        {
            this.shoppingCartItemService = shoppingCartItemService;
        }

        [HttpPost("Add", Name = "AddItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddCartItem([FromBody] CreateShoppingCartItemRequestDto request)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            var item = await shoppingCartItemService.AddToCartAsync(customerId, request);
            return CreatedAtAction(nameof(GetItemByID), new { ID = item.Id }, item);
        }

        [HttpGet("GetByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetItemByID([FromQuery] Guid ID)
        {
            var item = await shoppingCartItemService.GetByIdAsync(ID);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetItemsByCartIdAsync([FromRoute] Guid ID)
        {
            var items = await shoppingCartItemService.GetByCartIdAsync(ID);
            return Ok(items);
        }

        [HttpPut("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateItem([FromRoute] Guid ID, [FromBody] UpdateShoppingCartItemRequestDto request)
        {
            var item = await shoppingCartItemService.UpdateQuantityAsync(ID, request);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> DeleteItem([FromRoute] Guid ID)
        {
            var item = await shoppingCartItemService.DeleteAsync(ID);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
    }
}
