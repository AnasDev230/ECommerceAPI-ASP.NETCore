using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Rating;

namespace ECommerceAPI_ASP.NETCore.Services.Interface
{
    public interface IRatingService
    {
        Task<RatingDto> CreateAsync(string customerId, CreateRatingRequestDto request);
        Task<RatingDto?> GetByCustomerAndProductAsync(string customerId, Guid productId);
        Task<IEnumerable<RatingDto>> GetByProductIdAsync(Guid productId);
        Task<RatingDto?> UpdateAsync(string customerId, Guid productId, UpdateRatingRequestDto request);
        Task<RatingDto?> DeleteAsync(string customerId, Guid productId);
    }
}
