using ECommerceAPI_ASP.NETCore.Models.DTO.Payment;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequestDto request)
        {
            var payment = await paymentService.CreateAsync(request);
            return Created("", payment);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetPaymentById([FromRoute] Guid id)
        {
            var payment = await paymentService.GetByIdAsync(id);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

        [HttpGet("Order/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetPaymentByOrderId([FromRoute] Guid orderId)
        {
            var payment = await paymentService.GetByOrderIdAsync(orderId);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await paymentService.GetAllAsync();
            return Ok(payments);
        }

        [HttpGet("Status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaymentsByStatus([FromRoute] string status)
        {
            var payments = await paymentService.GetByStatusAsync(status);
            return Ok(payments);
        }

        [HttpPut("{paymentId}/Status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePaymentStatus([FromRoute] Guid paymentId, [FromBody] UpdatePaymentStatusRequestDto request)
        {
            var payment = await paymentService.UpdateStatusAsync(paymentId, request);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePayment([FromRoute] Guid id)
        {
            var deleted = await paymentService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return Ok("Payment deleted successfully");
        }
    }
}
