using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Address;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository addressRepository;
        private readonly IMapper mapper;

        public AddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            this.addressRepository = addressRepository;
            this.mapper = mapper;
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
            return mapper.Map<AddressDto>(address);
        }

        public async Task<IEnumerable<AddressDto>> GetAllByUserIdAsync(string userId)
        {
            var addresses = await addressRepository.GetAllByUserIdAsync(userId);
            return mapper.Map<IEnumerable<AddressDto>>(addresses);
        }

        public async Task<AddressDto?> GetByIdAsync(Guid id, string userId)
        {
            var address = await addressRepository.GetByIdAsync(id);
            if (address == null || address.UserId != userId)
                return null;

            return mapper.Map<AddressDto>(address);
        }

        public async Task<AddressDto?> GetDefaultAsync(string userId)
        {
            var address = await addressRepository.GetDefaultByUserIdAsync(userId);
            if (address == null)
                return null;

            return mapper.Map<AddressDto>(address);
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
            return updatedAddress != null ? mapper.Map<AddressDto>(updatedAddress) : null;
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
            return address != null ? mapper.Map<AddressDto>(address) : null;
        }
    }
}
