using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Shipping;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class ShippingService : IShippingService
    {
        private readonly IShippingRepository shippingRepository;
        private readonly IOrderRepository orderRepository;

        public ShippingService(IShippingRepository shippingRepository, IOrderRepository orderRepository)
        {
            this.shippingRepository = shippingRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<ShippingDto> CreateAsync(CreateShippingRequestDto request)
        {
            var order = await orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");

            var existingShipping = await shippingRepository.GetByOrderIdAsync(request.OrderId);
            if (existingShipping != null)
                throw new InvalidOperationException("Shipping already exists for this order.");

            var shipping = new Shipping
            {
                OrderId = request.OrderId,
                Carrier = request.Carrier,
                TrackingNumber = request.TrackingNumber,
                EstimatedDelivery = request.EstimatedDelivery,
                Notes = request.Notes,
                ShippingAddressId = request.ShippingAddressId,
                Status = ShippingStatus.Pending,
            };

            await shippingRepository.CreateAsync(shipping);
            return MapToDto(shipping);
        }

        public async Task<ShippingDto?> GetByIdAsync(Guid id)
        {
            var shipping = await shippingRepository.GetByIdAsync(id);
            if (shipping == null)
                return null;

            return MapToDto(shipping);
        }

        public async Task<ShippingDto?> GetByOrderIdAsync(Guid orderId)
        {
            var shipping = await shippingRepository.GetByOrderIdAsync(orderId);
            if (shipping == null)
                return null;

            return MapToDto(shipping);
        }

        public async Task<ShippingDto?> GetByTrackingNumberAsync(string trackingNumber)
        {
            var shipping = await shippingRepository.GetByTrackingNumberAsync(trackingNumber);
            if (shipping == null)
                return null;

            return MapToDto(shipping);
        }

        public async Task<IEnumerable<ShippingDto>> GetAllAsync()
        {
            var shippings = await shippingRepository.GetAllAsync();
            return shippings.Select(MapToDto);
        }

        public async Task<IEnumerable<ShippingDto>> GetByStatusAsync(string status)
        {
            if (!Enum.TryParse<ShippingStatus>(status, true, out var shippingStatus))
                throw new ArgumentException($"Invalid shipping status: {status}");

            var shippings = await shippingRepository.GetByStatusAsync(shippingStatus);
            return shippings.Select(MapToDto);
        }

        public async Task<ShippingDto?> UpdateAsync(Guid id, UpdateShippingRequestDto request)
        {
            var shipping = await shippingRepository.GetByIdAsync(id);
            if (shipping == null)
                return null;

            shipping.Carrier = request.Carrier;
            shipping.TrackingNumber = request.TrackingNumber;
            shipping.EstimatedDelivery = request.EstimatedDelivery;
            shipping.ActualDelivery = request.ActualDelivery;
            shipping.Notes = request.Notes;
            shipping.ShippingAddressId = request.ShippingAddressId;

            var updated = await shippingRepository.UpdateAsync(shipping);
            if (!updated)
                return null;

            var updatedShipping = await shippingRepository.GetByIdAsync(id);
            return updatedShipping != null ? MapToDto(updatedShipping) : null;
        }

        public async Task<ShippingDto?> UpdateStatusAsync(Guid shippingId, UpdateShippingStatusRequestDto request)
        {
            var shipping = await shippingRepository.GetByIdAsync(shippingId);
            if (shipping == null)
                return null;

            if (shipping.Status is ShippingStatus.Delivered or ShippingStatus.Returned or ShippingStatus.Failed)
                throw new InvalidOperationException("Shipping status cannot be updated from its current state.");

            var updated = await shippingRepository.UpdateStatusAsync(shippingId, request.Status);
            if (!updated)
                return null;

            var updatedShipping = await shippingRepository.GetByIdAsync(shippingId);
            return updatedShipping != null ? MapToDto(updatedShipping) : null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var shipping = await shippingRepository.GetByIdAsync(id);
            if (shipping == null)
                throw new KeyNotFoundException($"Shipping with ID {id} not found.");

            if (shipping.Status is ShippingStatus.InTransit or ShippingStatus.OutForDelivery or ShippingStatus.Delivered)
                throw new InvalidOperationException("Cannot delete shipping that is in transit, out for delivery, or delivered.");

            return await shippingRepository.DeleteAsync(id);
        }

        private static ShippingDto MapToDto(Shipping shipping)
        {
            return new ShippingDto
            {
                Id = shipping.Id,
                OrderId = shipping.OrderId,
                Carrier = shipping.Carrier,
                TrackingNumber = shipping.TrackingNumber,
                EstimatedDelivery = shipping.EstimatedDelivery,
                ActualDelivery = shipping.ActualDelivery,
                Status = shipping.Status.ToString(),
                Notes = shipping.Notes,
                ShippingAddressId = shipping.ShippingAddressId,
                CreatedAt = shipping.CreatedAt,
            };
        }
    }
}
