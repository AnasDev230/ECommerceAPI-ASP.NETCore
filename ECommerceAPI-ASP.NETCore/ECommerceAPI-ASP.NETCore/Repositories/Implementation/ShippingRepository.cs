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

        public async Task<Shipping?> DeleteAsync(Guid id)
        {
            var shipping = await dbContext.Shippings.FindAsync(id);
            if (shipping == null)
                return null;

            dbContext.Shippings.Remove(shipping);
            await dbContext.SaveChangesAsync();
            return shipping;
        }

        public async Task<IEnumerable<Shipping>> GetAllAsync()
        {
            return await dbContext.Shippings
                .Include(s => s.Order)
                .Include(s => s.ShippingAddress)
                .ToListAsync();
        }

        public async Task<Shipping?> GetByIdAsync(Guid id)
        {
            return await dbContext.Shippings
                .Include(s => s.Order)
                .Include(s => s.ShippingAddress)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Shipping?> GetByOrderIdAsync(Guid orderId)
        {
            return await dbContext.Shippings
                .Include(s => s.Order)
                .Include(s => s.ShippingAddress)
                .FirstOrDefaultAsync(s => s.OrderId == orderId);
        }

        public async Task<Shipping?> GetByTrackingNumberAsync(string trackingNumber)
        {
            return await dbContext.Shippings
                .Include(s => s.Order)
                .Include(s => s.ShippingAddress)
                .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);
        }

        public async Task<IEnumerable<Shipping>> GetByStatusAsync(ShippingStatus status)
        {
            return await dbContext.Shippings
                .Include(s => s.Order)
                .Include(s => s.ShippingAddress)
                .Where(s => s.Status == status)
                .ToListAsync();
        }

        public async Task<Shipping?> UpdateAsync(Shipping shipping)
        {
            var existingShipping = await dbContext.Shippings.FirstOrDefaultAsync(s => s.Id == shipping.Id);
            if (existingShipping == null)
                return null;

            existingShipping.Carrier = shipping.Carrier;
            existingShipping.TrackingNumber = shipping.TrackingNumber;
            existingShipping.EstimatedDelivery = shipping.EstimatedDelivery;
            existingShipping.ActualDelivery = shipping.ActualDelivery;
            existingShipping.Status = shipping.Status;
            existingShipping.Notes = shipping.Notes;
            existingShipping.ShippingAddressId = shipping.ShippingAddressId;
            existingShipping.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return existingShipping;
        }

        public async Task<Shipping?> UpdateStatusAsync(Guid shippingId, ShippingStatus newStatus)
        {
            var existingShipping = await dbContext.Shippings.FindAsync(shippingId);
            if (existingShipping == null)
                return null;

            existingShipping.Status = newStatus;
            existingShipping.UpdatedAt = DateTime.UtcNow;

            if (newStatus == ShippingStatus.Delivered)
                existingShipping.ActualDelivery = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return existingShipping;
        }
    }
}
