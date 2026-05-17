using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class AddressRepository : IAddressRepository
    {
        private readonly EcommerceDBContext dbContext;

        public AddressRepository(EcommerceDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Address> CreateAsync(Address address)
        {
            if (address.IsDefault)
            {
                await dbContext.Addresses
                    .Where(a => a.UserId == address.UserId && a.IsDefault)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(a => a.IsDefault, false));
            }

            await dbContext.Addresses.AddAsync(address);
            await dbContext.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var rowsAffected = await dbContext.Addresses
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Address>> GetAllByUserIdAsync(string userId)
        {
            return await dbContext.Addresses
                .AsNoTracking()
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<Address?> GetByIdAsync(Guid id)
        {
            return await dbContext.Addresses
                .AsNoTracking()
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Address?> GetDefaultByUserIdAsync(string userId)
        {
            return await dbContext.Addresses
                .AsNoTracking()
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);
        }

        public async Task<bool> SetDefaultAsync(Guid addressId, string userId)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var addressExists = await dbContext.Addresses.AnyAsync(a => a.Id == addressId && a.UserId == userId);
                if (!addressExists)
                    return false;

                await dbContext.Addresses
                    .Where(a => a.UserId == userId && a.Id != addressId)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(a => a.IsDefault, false));

                await dbContext.Addresses
                    .Where(a => a.Id == addressId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(a => a.IsDefault, true)
                        .SetProperty(a => a.UpdatedAt, DateTime.UtcNow));

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Address address)
        {
            var rowsAffected = await dbContext.Addresses
                .Where(a => a.Id == address.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.Street, address.Street)
                    .SetProperty(a => a.City, address.City)
                    .SetProperty(a => a.State, address.State)
                    .SetProperty(a => a.PostalCode, address.PostalCode)
                    .SetProperty(a => a.Country, address.Country)
                    .SetProperty(a => a.PhoneNumber, address.PhoneNumber)
                    .SetProperty(a => a.AddressType, address.AddressType)
                    .SetProperty(a => a.IsDefault, address.IsDefault)
                    .SetProperty(a => a.UpdatedAt, DateTime.UtcNow));

            return rowsAffected > 0;
        }
    }
}
