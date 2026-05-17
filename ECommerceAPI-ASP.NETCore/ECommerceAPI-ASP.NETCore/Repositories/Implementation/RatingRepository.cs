using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class RatingRepository : IRatingRepository
    {
        private readonly EcommerceDBContext dbContext;

        public RatingRepository(EcommerceDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Rating> AddRatingAsync(Rating rating)
        {
            await dbContext.Ratings.AddAsync(rating);
            await dbContext.SaveChangesAsync();
            return rating;
        }

        public async Task<bool> DeleteRatingAsync(Guid ratingId)
        {
            var rowsAffected = await dbContext.Ratings
                .Where(r => r.Id == ratingId)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> ExistsAsync(Guid productId, string customerId)
        {
            return await dbContext.Ratings.AnyAsync(r => r.ProductId == productId && r.CustomerId == customerId);
        }

        public async Task<Rating?> GetRatingAsync(Guid productId, string customerId)
        {
            return await dbContext.Ratings
                .AsNoTracking()
                .Include(r => r.Product)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.CustomerId == customerId);
        }

        public async Task<IEnumerable<Rating>> GetRatingsByProductAsync(Guid productId)
        {
            return await dbContext.Ratings
                .AsNoTracking()
                .Include(r => r.Product)
                .Include(r => r.Customer)
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }

        public async Task<bool> UpdateRatingAsync(Rating rating)
        {
            var rowsAffected = await dbContext.Ratings
                .Where(r => r.Id == rating.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(r => r.Stars, rating.Stars)
                    .SetProperty(r => r.Comment, rating.Comment)
                    .SetProperty(r => r.IsVerifiedPurchase, rating.IsVerifiedPurchase)
                    .SetProperty(r => r.UpdatedAt, DateTime.UtcNow));

            return rowsAffected > 0;
        }
    }
}
