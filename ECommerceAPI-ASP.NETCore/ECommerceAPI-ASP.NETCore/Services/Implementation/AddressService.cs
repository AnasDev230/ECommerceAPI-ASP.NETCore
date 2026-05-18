using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Address;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
        }

        public async Task<AddressDto> CreateAsync(string userId, CreateAddressRequestDto request)
        {
            var address = new Address
            {
                UserId = userId,
                Street = request.Street,
                City = request.City,
                State = request.State,
                PostalCode = request.PostalCode,
                Country = request.Country,
                PhoneNumber = request.PhoneNumber,
                IsDefault = request.IsDefault,
                AddressType = request.AddressType,
            };

            await addressRepository.CreateAsync(address);
            return MapToDto(address);
        }

        public async Task<IEnumerable<AddressDto>> GetAllByUserIdAsync(string userId)
        {
            var addresses = await addressRepository.GetAllByUserIdAsync(userId);
            return addresses.Select(MapToDto);
        }

        public async Task<AddressDto?> GetByIdAsync(Guid id, string userId)
        {
            var address = await addressRepository.GetByIdAsync(id);
            if (address == null || address.UserId != userId)
                return null;

            return MapToDto(address);
        }

        public async Task<AddressDto?> GetDefaultAsync(string userId)
        {
            var address = await addressRepository.GetDefaultByUserIdAsync(userId);
            if (address == null)
                return null;

            return MapToDto(address);
        }

        public async Task<AddressDto?> UpdateAsync(Guid id, string userId, UpdateAddressRequestDto request)
        {
            var address = await addressRepository.GetByIdAsync(id);
            if (address == null || address.UserId != userId)
                return null;

            if (request.Street != null) address.Street = request.Street;
            if (request.City != null) address.City = request.City;
            if (request.State != null) address.State = request.State;
            if (request.PostalCode != null) address.PostalCode = request.PostalCode;
            if (request.Country != null) address.Country = request.Country;
            if (request.PhoneNumber != null) address.PhoneNumber = request.PhoneNumber;
            if (request.AddressType.HasValue) address.AddressType = request.AddressType.Value;

            var updated = await addressRepository.UpdateAsync(address);
            if (!updated)
                return null;

            var updatedAddress = await addressRepository.GetByIdAsync(id);
            return updatedAddress != null ? MapToDto(updatedAddress) : null;
        }

        public async Task<bool> DeleteAsync(Guid id, string userId)
        {
            var address = await addressRepository.GetByIdAsync(id);
            if (address == null || address.UserId != userId)
                return false;

            return await addressRepository.DeleteAsync(id);
        }

        public async Task<AddressDto?> SetDefaultAsync(Guid addressId, string userId)
        {
            var updated = await addressRepository.SetDefaultAsync(addressId, userId);
            if (!updated)
                return null;

            var address = await addressRepository.GetByIdAsync(addressId);
            return address != null ? MapToDto(address) : null;
        }

        private static AddressDto MapToDto(Address address)
        {
            return new AddressDto
            {
                Id = address.Id,
                UserId = address.UserId,
                Street = address.Street,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                PhoneNumber = address.PhoneNumber,
                IsDefault = address.IsDefault,
                AddressType = address.AddressType.ToString(),
                CreatedAt = address.CreatedAt,
            };
        }
    }
}
