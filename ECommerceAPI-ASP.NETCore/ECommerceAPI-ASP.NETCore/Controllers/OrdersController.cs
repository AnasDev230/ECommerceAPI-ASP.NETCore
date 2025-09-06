using System.Security.Claims;
using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.DTO.Order.OrderItem;
using ECommerceAPI_ASP.NETCore.Models.DTO.Order;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public OrdersController(IOrderRepository orderRepository,IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();

            try
            {

                var order=await orderRepository.CreateOrderAsync(customerId);
                var response = new OrderDto
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    CreatedAt = order.CreatedAt,
                    TotalAmount=order.TotalAmount,
                    Status = order.Status,
                    Items = order.Items.Select(i => new OrderItemDto
                    {
                        Id = i.Id,
                        StockId = i.StockId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        TotalPrice = i.TotalPrice,
                        
                    }).ToList()

                };

                return Ok(response);



            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }


        }


        [HttpGet("{orderID}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetOrderByID([FromRoute] Guid orderID)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();
            var order=await orderRepository.GetOrderByIdAsync(orderID);
            if(order == null)
                return NotFound();
            return Ok(mapper.Map<OrderDto>(order));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await orderRepository.GetAllOrdersAsync();
            return Ok(mapper.Map<List<OrderDto>>(orders));
        }

        [HttpPut("{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] Guid orderId, [FromBody] UpdateOrderStatusRequestDto request)
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound();
            if(request.Status is "Pending" or "Paid" or "Shipped" or "Completed" or "Cancelled")
            {
                order.Status = request.Status;
                order = await orderRepository.UpdateOrderStatusAsync(order);
                return Ok(mapper.Map<OrderDto>(order));
            }

            return BadRequest("Incorrect Status!!");
        }

        [HttpDelete("{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid orderId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null)
                return Unauthorized();
            var order = await orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound();

            if(await orderRepository.DeleteOrderAsync(orderId))
                return Ok("Order Deleted Successfully");
            return BadRequest();

        }
    }
}
