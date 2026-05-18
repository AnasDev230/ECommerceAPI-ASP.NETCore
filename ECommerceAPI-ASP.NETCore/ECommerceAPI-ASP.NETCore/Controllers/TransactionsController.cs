using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public TransactionsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost("{paymentId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTransaction(
            [FromRoute] Guid paymentId,
            [FromQuery] string gatewayTransactionId,
            [FromQuery] string type,
            [FromQuery] decimal amount,
            [FromQuery] string? gatewayRawResponse = null)
        {
            if (string.IsNullOrWhiteSpace(gatewayTransactionId))
                return BadRequest("Gateway transaction ID is required.");

            if (string.IsNullOrWhiteSpace(type))
                return BadRequest("Transaction type is required.");

            try
            {
                var transaction = await paymentService.AddTransactionAsync(paymentId, gatewayTransactionId, type, amount, gatewayRawResponse);
                return Created("", transaction);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTransactionsByPaymentId([FromRoute] Guid paymentId)
        {
            try
            {
                var transactions = await paymentService.GetTransactionsByPaymentIdAsync(paymentId);
                return Ok(transactions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
