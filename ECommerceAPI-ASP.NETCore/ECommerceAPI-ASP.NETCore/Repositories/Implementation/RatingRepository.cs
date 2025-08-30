using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class RatingRepository : IRatingRepository
    {
        private readonly EcommerceDBContext dBContext;

        public RatingRepository(EcommerceDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Rating> AddRatingAsync(Rating rating)
        {
            await dBContext.Ratings.AddAsync(rating);
            await dBContext.SaveChangesAsync();
            return rating;
        }

        public async Task<bool> DeleteRatingAsync(Guid ratingId)
        {
            var rating = await dBContext.Ratings.FindAsync(ratingId);
            if (rating == null)
                return false;
             dBContext.Ratings.Remove(rating);
            await dBContext.SaveChangesAsync();
            return true;
        }

        public async Task<Rating?> GetRatingAsync(Guid productId, string customerId)
        {
            return await dBContext.Ratings.FirstOrDefaultAsync(x=>x.ProductId==productId&& x.CustomerId==customerId);
        }

        public async Task<IEnumerable<Rating>> GetRatingsByProductAsync(Guid productId)
        {
            return await dBContext.Ratings.Where(x=>x.ProductId==productId).ToListAsync();
        }

        public async Task<Rating?> UpdateRatingAsync(Rating rating)
        {
            var existingRating=await dBContext.Ratings.FirstOrDefaultAsync(r => r.Id==rating.Id && r.CustomerId==rating.CustomerId);
            if (existingRating == null)
                return null;
            existingRating.Stars=rating.Stars;
            existingRating.Comment=rating.Comment;
            await dBContext.SaveChangesAsync(); 
            return existingRating;
        }
    }
}
