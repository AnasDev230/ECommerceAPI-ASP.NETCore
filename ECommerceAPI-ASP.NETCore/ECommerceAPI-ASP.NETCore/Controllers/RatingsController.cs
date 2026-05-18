using System.Security.Claims;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Rating;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService ratingService;

        public RatingsController(IRatingService ratingService)
        {
            this.ratingService = ratingService;
        }

        [HttpPost("Add", Name = "AddRating")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> AddRating([FromBody] CreateRatingRequestDto request)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            var rating = await ratingService.CreateAsync(customerId, request);
            return CreatedAtAction(nameof(GetMyRating), new { productId = request.ProductId }, rating);
        }

        [HttpGet("MyRating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetMyRating([FromQuery] Guid productId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            var rating = await ratingService.GetByCustomerAndProductAsync(customerId, productId);
            if (rating == null)
                return NotFound("You have not rated this product yet.");
            return Ok(rating);
        }

        [HttpGet("{productID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> GetRatingsByProduct([FromRoute] Guid productID)
        {
            var ratings = await ratingService.GetByProductIdAsync(productID);
            return Ok(ratings);
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> UpdateRating([FromRoute] Guid productId, [FromBody] UpdateRatingRequestDto request)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            var rating = await ratingService.UpdateAsync(customerId, productId, request);
            if (rating == null)
                return NotFound();
            return Ok(rating);
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Vendor,Customer")]
        public async Task<IActionResult> DeleteRating([FromRoute] Guid productId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            var rating = await ratingService.DeleteAsync(customerId, productId);
            if (rating == null)
                return NotFound();
            return Ok(rating);
        }
    }
}
