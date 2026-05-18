using System.Security.Claims;
using ECommerceAPI_ASP.NETCore.Models.DTO.Address;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService addressService;

        public AddressesController(IAddressService addressService)
        {
            this.addressService = addressService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var address = await addressService.CreateAsync(userId, request);
            return Created("", address);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetAllAddresses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var addresses = await addressService.GetAllByUserIdAsync(userId);
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetAddressById([FromRoute] Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var address = await addressService.GetByIdAsync(id, userId);
            if (address == null)
                return NotFound();
            return Ok(address);
        }

        [HttpGet("Default")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetDefaultAddress()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var address = await addressService.GetDefaultAsync(userId);
            if (address == null)
                return NotFound();
            return Ok(address);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> UpdateAddress([FromRoute] Guid id, [FromBody] UpdateAddressRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var address = await addressService.UpdateAsync(id, userId, request);
            if (address == null)
                return NotFound();
            return Ok(address);
        }

        [HttpPut("{id}/SetDefault")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> SetDefaultAddress([FromRoute] Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var address = await addressService.SetDefaultAsync(id, userId);
            if (address == null)
                return NotFound();
            return Ok(address);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> DeleteAddress([FromRoute] Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var deleted = await addressService.DeleteAsync(id, userId);
            if (!deleted)
                return NotFound();
            return Ok("Address deleted successfully");
        }
    }
}
