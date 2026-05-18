using ECommerceAPI_ASP.NETCore.Models.DTO.Shipping;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingsController : ControllerBase
    {
        private readonly IShippingService shippingService;

        public ShippingsController(IShippingService shippingService)
        {
            this.shippingService = shippingService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateShipping([FromBody] CreateShippingRequestDto request)
        {
            try
            {
                var shipping = await shippingService.CreateAsync(request);
                return Created("", shipping);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetShippingById([FromRoute] Guid id)
        {
            var shipping = await shippingService.GetByIdAsync(id);
            if (shipping == null)
                return NotFound();
            return Ok(shipping);
        }

        [HttpGet("Order/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetShippingByOrderId([FromRoute] Guid orderId)
        {
            var shipping = await shippingService.GetByOrderIdAsync(orderId);
            if (shipping == null)
                return NotFound();
            return Ok(shipping);
        }

        [HttpGet("Tracking/{trackingNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetShippingByTrackingNumber([FromRoute] string trackingNumber)
        {
            var shipping = await shippingService.GetByTrackingNumberAsync(trackingNumber);
            if (shipping == null)
                return NotFound();
            return Ok(shipping);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllShippings()
        {
            var shippings = await shippingService.GetAllAsync();
            return Ok(shippings);
        }

        [HttpGet("Status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetShippingsByStatus([FromRoute] string status)
        {
            try
            {
                var shippings = await shippingService.GetByStatusAsync(status);
                return Ok(shippings);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateShipping([FromRoute] Guid id, [FromBody] UpdateShippingRequestDto request)
        {
            var shipping = await shippingService.UpdateAsync(id, request);
            if (shipping == null)
                return NotFound();
            return Ok(shipping);
        }

        [HttpPut("{shippingId}/Status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateShippingStatus([FromRoute] Guid shippingId, [FromBody] UpdateShippingStatusRequestDto request)
        {
            try
            {
                var shipping = await shippingService.UpdateStatusAsync(shippingId, request);
                if (shipping == null)
                    return NotFound();
                return Ok(shipping);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteShipping([FromRoute] Guid id)
        {
            try
            {
                var deleted = await shippingService.DeleteAsync(id);
                if (!deleted)
                    return NotFound();
                return Ok("Shipping deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
