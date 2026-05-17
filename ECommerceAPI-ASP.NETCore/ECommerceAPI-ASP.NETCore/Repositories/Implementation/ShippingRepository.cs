using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class ShippingRepository : IShippingRepository
    {
        private readonly EcommerceDBContext dbContext;

        public ShippingRepository(EcommerceDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Shipping> CreateAsync(Shipping shipping)
        {
            await dbContext.Shippings.AddAsync(shipping);
            await dbContext.SaveChangesAsync();
            return shipping;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var rowsAffected = await dbContext.Shippings
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Shipping>> GetAllAsync()
        {
            return await dbContext.Shippings
                .AsNoTracking()
                .Include(s => s.Order)
                .Include(s => s.ShippingAddress)
                .ToListAsync();
        }

        public async Task<Shipping?> GetByIdAsync(Guid id)
        {
            return await dbContext.Shippings
                .AsNoTracking()
                .Include(s => s.Order)
                .Include(s => s.ShippingAddress)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Shipping?> GetByOrderIdAsync(Guid orderId)
        {
            return await dbContext.Shippings
                .AsNoTracking()
                .Include(s => s.Order)
                .Include(s => s.ShippingAddress)
                .FirstOrDefaultAsync(s => s.OrderId == orderId);
        }

        public async Task<Shipping?> GetByTrackingNumberAsync(string trackingNumber)
        {
            return await dbContext.Shippings
                .AsNoTracking()
                .Include(s => s.Order)
                .Include(s => s.ShippingAddress)
                .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);
        }

        public async Task<IEnumerable<Shipping>> GetByStatusAsync(ShippingStatus status)
        {
            return await dbContext.Shippings
                .AsNoTracking()
                .Include(s => s.Order)
                .Include(s => s.ShippingAddress)
                .Where(s => s.Status == status)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Shipping shipping)
        {
            var rowsAffected = await dbContext.Shippings
                .Where(s => s.Id == shipping.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.Carrier, shipping.Carrier)
                    .SetProperty(s => s.TrackingNumber, shipping.TrackingNumber)
                    .SetProperty(s => s.EstimatedDelivery, shipping.EstimatedDelivery)
                    .SetProperty(s => s.ActualDelivery, shipping.ActualDelivery)
                    .SetProperty(s => s.Status, shipping.Status)
                    .SetProperty(s => s.Notes, shipping.Notes)
                    .SetProperty(s => s.ShippingAddressId, shipping.ShippingAddressId)
                    .SetProperty(s => s.UpdatedAt, DateTime.UtcNow));

            return rowsAffected > 0;
        }

        public async Task<bool> UpdateStatusAsync(Guid shippingId, ShippingStatus newStatus)
        {
            if (newStatus == ShippingStatus.Delivered)
            {
                var rowsAffected = await dbContext.Shippings
                    .Where(s => s.Id == shippingId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.Status, newStatus)
                        .SetProperty(s => s.UpdatedAt, DateTime.UtcNow)
                        .SetProperty(s => s.ActualDelivery, DateTime.UtcNow));

                return rowsAffected > 0;
            }

            var rows = await dbContext.Shippings
                .Where(s => s.Id == shippingId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.Status, newStatus)
                    .SetProperty(s => s.UpdatedAt, DateTime.UtcNow));

            return rows > 0;
        }
    }
}
