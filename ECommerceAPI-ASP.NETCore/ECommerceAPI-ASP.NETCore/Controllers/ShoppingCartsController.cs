using System.Security.Claims;
using AutoMapper;
using Blog_API.Repositories.Implementation;
using Blog_API.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Category;
using ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IMapper mapper;

        public ShoppingCartsController(IShoppingCartRepository shoppingCartRepository,IMapper mapper)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.mapper = mapper;
        }
        [HttpPost("Add", Name = "AddShoppingCart")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddShoppingCart()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();
            var shoppingCart = new ShoppingCart
            {
                CustomerId = customerId,
            };

            await shoppingCartRepository.CreateAsync(shoppingCart);
            return Created("", mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpGet("GetByCustomerID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCartByCustomerIdAsync()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();
            var shoppingCart=await shoppingCartRepository.GetCartByCustomerIdAsync(customerId);
            if(shoppingCart == null)
                return NotFound("Shopping Cart is Empty");
            return Ok(mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCartByID(Guid shoppingCartID)
        {
            var shoppingCart=shoppingCartRepository.GetCartByID(shoppingCartID);
            if(shoppingCart == null)
                return NotFound();
            return Ok(mapper.Map<ShoppingCartDto>(shoppingCart));
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteShoppingCart()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();
            var shoppingCart = await shoppingCartRepository.GetCartByCustomerIdAsync(customerId);
            if (shoppingCart == null)
                return NotFound();
            shoppingCart = await shoppingCartRepository.DeleteAsync(shoppingCart.Id);
            return Ok(mapper.Map<ShoppingCartDto>(shoppingCart));
        }


    }
}
