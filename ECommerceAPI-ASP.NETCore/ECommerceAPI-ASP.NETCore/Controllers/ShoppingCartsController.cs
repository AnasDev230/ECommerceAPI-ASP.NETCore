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
        public async Task<IActionResult> AddShoppingCart()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingCart = new ShoppingCart
            {
                CustomerId = customerId,
            };

            await shoppingCartRepository.CreateAsync(shoppingCart);
            return Created("", mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpGet("GetByCustomerID")]
        //[Authorize(Roles = "Vendor")]
        public async Task<IActionResult> GetCartByCustomerIdAsync()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return BadRequest();
            var shoppingCart=await shoppingCartRepository.GetCartByCustomerIdAsync(customerId);
            return Ok(mapper.Map<ShoppingCartDto>(shoppingCart));
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteShoppingCart()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingCart = await shoppingCartRepository.GetCartByCustomerIdAsync(customerId);
            if (shoppingCart == null)
                return NotFound();
            shoppingCart = await shoppingCartRepository.DeleteAsync(shoppingCart.Id);
            return Ok(mapper.Map<ShoppingCartDto>(shoppingCart));
        }
    }
}
