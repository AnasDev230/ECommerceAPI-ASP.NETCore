using System.Security.Claims;
using AutoMapper;
using Azure.Core;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Rating;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingRepository ratingRepository;
        private readonly IMapper mapper;
        public RatingsController(IRatingRepository ratingRepository, IMapper mapper)
        {
            this.ratingRepository = ratingRepository;
            this.mapper = mapper;
        }
        [HttpPost("Add", Name = "AddRating")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddRating([FromBody] CreateRatingRequestDto request)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();
            var existing = await ratingRepository.GetRatingAsync(request.ProductId, customerId);
            if (existing != null)
                return BadRequest("You have already rated this product.");
            var rating = new Rating
            {
                ProductId = request.ProductId,
                CustomerId = customerId,
                Stars = request.Stars,
                Comment = request.Comment,
                CreatedAt = DateTime.Now,
            };
            await ratingRepository.AddRatingAsync(rating);
            return Created("", mapper.Map<RatingDto>(rating));
        }
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyRating([FromBody] GetMyRatingRequestDto request)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();
            var rating = await ratingRepository.GetRatingAsync(request.ProductId, customerId);
            if (rating == null)
                return NotFound("You have not rated this product yet.");
            return Ok(mapper.Map<RatingDto>(rating));

        }

        [HttpGet("{productID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetRatingsByProduct([FromRoute] Guid productID)
        {
            var ratings = ratingRepository.GetRatingsByProductAsync(productID);
            return Ok(mapper.Map<List<RatingDto>>(ratings));
        }

        [HttpPut("{productId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateRating([FromRoute] Guid productId,[FromBody] UpdateRatingRequestDto request)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();
            var rating=await ratingRepository.GetRatingAsync(productId, customerId);
            if (rating == null)
                return NotFound();
            mapper.Map(request,rating);
            rating =await ratingRepository.UpdateRatingAsync(rating);
            return Ok(mapper.Map<RatingDto>(rating));
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteRating([FromRoute] Guid productId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();
            var rating = await ratingRepository.GetRatingAsync(productId, customerId);
            if (rating == null)
                return NotFound();
           await ratingRepository.DeleteRatingAsync(rating.Id);
            return Ok(mapper.Map<RatingDto>(rating));
        }


    }
}
