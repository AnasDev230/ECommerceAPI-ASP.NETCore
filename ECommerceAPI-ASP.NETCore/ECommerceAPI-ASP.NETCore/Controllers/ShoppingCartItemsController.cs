using System.Security.Claims;
using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart.ShoppingCartItem;
using ECommerceAPI_ASP.NETCore.Repositories.Implementation;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartItemsController : ControllerBase
    {
        private readonly IShoppingCartItemRepository shoppingCartItemRepository;
        private readonly IMapper mapper;
        private readonly IShoppingCartRepository shoppingCartRepository;
        public ShoppingCartItemsController(IShoppingCartItemRepository shoppingCartItemRepository,IMapper mapper,IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartItemRepository = shoppingCartItemRepository;
            this.mapper = mapper;
            this.shoppingCartRepository = shoppingCartRepository;
        }
        [HttpPost("Add", Name = "AddItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> AddCartItem([FromBody] CreateShoppingCartItemRequestDto request)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            var shoppingCart = await shoppingCartRepository.GetCartByCustomerIdAsync(customerId);
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    CreatedAt = DateTime.UtcNow
                };
                await shoppingCartRepository.CreateAsync(shoppingCart);
            }

            var item = new ShoppingCartItem
            {
                Id = Guid.NewGuid(),
                ShoppingCartId = shoppingCart.Id,
                StockId = request.StockId,
                Quantity = request.Quantity
            };
            await shoppingCartItemRepository.CreateAsync(item);

            return Created("", mapper.Map<ShoppingCartItemDto>(item));



        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetItemByID(Guid ID)
        {
            var item=shoppingCartItemRepository.GetItemsByCartIdAsync(ID);
            if(item == null)
                return NotFound();
            return Ok(mapper.Map<ShoppingCartItemDto>(item));
        }
        [HttpGet("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetItemsByCartIdAsync([FromRoute] Guid ID)
        {
            var items = shoppingCartItemRepository.GetItemsByCartIdAsync(ID);
            if(items == null)
                return NotFound();
            return Ok(mapper.Map<List<ShoppingCartItemDto>>(items));
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateItem(Guid ID,[FromBody] UpdateShoppingCartItemRequestDto request)
        {
            var item = await shoppingCartItemRepository.GetByIdAsync(ID);
            if(item == null)
                return NotFound();
            if (request.Quantity < 1)
                return BadRequest("Quantity must be at least 1.");
            item=await shoppingCartItemRepository.UpdateQuantityAsync(item.Id,request.Quantity);
            return Ok(mapper.Map<ShoppingCartItemDto>(item));
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> DeleteItem(Guid ID)
        {
            var item = await shoppingCartItemRepository.GetByIdAsync(ID);
            if (item == null)
                return NotFound();
            await shoppingCartItemRepository.DeleteAsync(item.Id);
            return Ok(mapper.Map<ShoppingCartItemDto>(item));
        }
    }
}
