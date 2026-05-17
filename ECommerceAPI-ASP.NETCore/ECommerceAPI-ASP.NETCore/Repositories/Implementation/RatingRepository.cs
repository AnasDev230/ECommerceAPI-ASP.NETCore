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
            var rating = await dbContext.Ratings.FindAsync(ratingId);
            if (rating == null)
                return false;

            dbContext.Ratings.Remove(rating);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid productId, string customerId)
        {
            return await dbContext.Ratings.AnyAsync(r => r.ProductId == productId && r.CustomerId == customerId);
        }

        public async Task<Rating?> GetRatingAsync(Guid productId, string customerId)
        {
            return await dbContext.Ratings
                .Include(r => r.Product)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.CustomerId == customerId);
        }

        public async Task<IEnumerable<Rating>> GetRatingsByProductAsync(Guid productId)
        {
            return await dbContext.Ratings
                .Where(x => x.ProductId == productId)
                .Include(x => x.Product)
                .Include(x => x.Customer)
                .ToListAsync();
        }

        public async Task<Rating?> UpdateRatingAsync(Rating rating)
        {
            var existingRating = await dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == rating.Id);
            if (existingRating == null)
                return null;

            existingRating.Stars = rating.Stars;
            existingRating.Comment = rating.Comment;
            existingRating.IsVerifiedPurchase = rating.IsVerifiedPurchase;
            existingRating.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return existingRating;
        }
    }
}
