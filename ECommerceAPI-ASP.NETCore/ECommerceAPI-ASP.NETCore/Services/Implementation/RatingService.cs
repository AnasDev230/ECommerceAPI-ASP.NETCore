using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Rating;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository ratingRepository;
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public RatingService(IRatingRepository ratingRepository, IProductRepository productRepository, IMapper mapper)
        {
            this.ratingRepository = ratingRepository;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<RatingDto> CreateAsync(string customerId, CreateRatingRequestDto request)
        {
            var product = await productRepository.GetByID(request.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");

            var existing = await ratingRepository.GetRatingAsync(request.ProductId, customerId);
            if (existing != null)
                throw new InvalidOperationException("You have already rated this product.");

            var rating = new Rating
            {
                ProductId = request.ProductId,
                CustomerId = customerId,
                Stars = request.Stars,
                Comment = request.Comment,
            };

            await ratingRepository.AddRatingAsync(rating);
            return mapper.Map<RatingDto>(rating);
        }

        public async Task<RatingDto?> GetByCustomerAndProductAsync(string customerId, Guid productId)
        {
            var rating = await ratingRepository.GetRatingAsync(productId, customerId);
            if (rating == null)
                return null;

            return mapper.Map<RatingDto>(rating);
        }

        public async Task<IEnumerable<RatingDto>> GetByProductIdAsync(Guid productId)
        {
            var product = await productRepository.GetByID(productId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            var ratings = await ratingRepository.GetRatingsByProductAsync(productId);
            return mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<RatingDto?> UpdateAsync(string customerId, Guid productId, UpdateRatingRequestDto request)
        {
            var rating = await ratingRepository.GetRatingAsync(productId, customerId);
            if (rating == null)
                return null;

            rating.Stars = request.Stars;
            rating.Comment = request.Comment;

            var updated = await ratingRepository.UpdateRatingAsync(rating);
            if (!updated)
                return null;

            var updatedRating = await ratingRepository.GetRatingAsync(productId, customerId);
            return mapper.Map<RatingDto>(updatedRating);
        }

        public async Task<RatingDto?> DeleteAsync(string customerId, Guid productId)
        {
            var rating = await ratingRepository.GetRatingAsync(productId, customerId);
            if (rating == null)
                return null;

            var deleted = await ratingRepository.DeleteRatingAsync(rating.Id);
            if (!deleted)
                return null;

            return mapper.Map<RatingDto>(rating);
        }
    }
}
