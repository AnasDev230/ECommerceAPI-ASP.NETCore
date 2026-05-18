using System.Security.Claims;
using ECommerceAPI_ASP.NETCore.Models.DTO.Order;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            var order = await orderService.CreateFromCartAsync(customerId);
            return Ok(order);
        }

        [HttpGet("{orderID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetOrderByID([FromRoute] Guid orderID)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            var order = await orderService.GetByIdAndCustomerAsync(orderID, customerId);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpPut("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] Guid orderId, [FromBody] UpdateOrderStatusRequestDto request)
        {
            var order = await orderService.UpdateStatusAsync(orderId, request);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpDelete("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid orderId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            var deleted = await orderService.DeleteAsync(orderId, customerId);
            if (!deleted)
                return NotFound();
            return Ok("Order Deleted Successfully");
        }
    }
}
