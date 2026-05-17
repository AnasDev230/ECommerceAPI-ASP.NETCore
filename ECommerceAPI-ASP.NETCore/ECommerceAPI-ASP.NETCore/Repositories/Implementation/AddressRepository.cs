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
                var existingDefault = await dbContext.Addresses
                    .FirstOrDefaultAsync(a => a.UserId == address.UserId && a.IsDefault);

                if (existingDefault != null)
                {
                    existingDefault.IsDefault = false;
                }
            }

            await dbContext.Addresses.AddAsync(address);
            await dbContext.SaveChangesAsync();
            return address;
        }

        public async Task<Address?> DeleteAsync(Guid id)
        {
            var address = await dbContext.Addresses.FindAsync(id);
            if (address == null)
                return null;

            dbContext.Addresses.Remove(address);
            await dbContext.SaveChangesAsync();
            return address;
        }

        public async Task<IEnumerable<Address>> GetAllByUserIdAsync(string userId)
        {
            return await dbContext.Addresses
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<Address?> GetByIdAsync(Guid id)
        {
            return await dbContext.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Address?> GetDefaultByUserIdAsync(string userId)
        {
            return await dbContext.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);
        }

        public async Task<Address?> SetDefaultAsync(Guid addressId, string userId)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var address = await dbContext.Addresses
                    .FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);

                if (address == null)
                    return null;

                var existingDefault = await dbContext.Addresses
                    .Where(a => a.UserId == userId && a.IsDefault && a.Id != addressId)
                    .ToListAsync();

                foreach (var defaultAddress in existingDefault)
                {
                    defaultAddress.IsDefault = false;
                }

                address.IsDefault = true;
                address.UpdatedAt = DateTime.UtcNow;

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return address;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Address?> UpdateAsync(Address address)
        {
            var existingAddress = await dbContext.Addresses.FirstOrDefaultAsync(a => a.Id == address.Id);
            if (existingAddress == null)
                return null;

            existingAddress.Street = address.Street;
            existingAddress.City = address.City;
            existingAddress.State = address.State;
            existingAddress.PostalCode = address.PostalCode;
            existingAddress.Country = address.Country;
            existingAddress.PhoneNumber = address.PhoneNumber;
            existingAddress.AddressType = address.AddressType;
            existingAddress.IsDefault = address.IsDefault;
            existingAddress.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return existingAddress;
        }
    }
}
