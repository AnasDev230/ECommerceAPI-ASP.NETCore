using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface IRatingRepository
    {
        Task<Rating?> GetRatingAsync(Guid productId, string customerId);
        Task<IEnumerable<Rating>> GetRatingsByProductAsync(Guid productId);
        Task<Rating> AddRatingAsync(Rating rating);
        Task<bool> UpdateRatingAsync(Rating rating);
        Task<bool> DeleteRatingAsync(Guid ratingId);
        Task<bool> ExistsAsync(Guid productId, string customerId);
    }
}
