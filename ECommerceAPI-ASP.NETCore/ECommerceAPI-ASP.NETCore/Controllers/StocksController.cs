using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Stock;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockService stockService;

        public StocksController(IStockService stockService)
        {
            this.stockService = stockService;
        }

        [HttpPost("Add", Name = "AddStock")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> AddStock([FromBody] CreateStockRequestDto request)
        {
            try
            {
                var stock = await stockService.CreateAsync(request);
                return Created("", stock);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{productID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> GetAllStocksByProductID([FromRoute] Guid productID)
        {
            try
            {
                var stocks = await stockService.GetByProductIdAsync(productID);
                return Ok(stocks);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetByID/{StockID}", Name = "GetStockByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> GetByID([FromRoute] Guid StockID)
        {
            var stock = await stockService.GetByIdAsync(StockID);
            if (stock == null)
                return NotFound();
            return Ok(stock);
        }

        [HttpPut("{StockID:Guid}", Name = "EditStock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> UpdateStock([FromRoute] Guid StockID, [FromBody] UpdateStockRequestDto request)
        {
            var stock = await stockService.UpdateAsync(StockID, request);
            if (stock == null)
                return NotFound();
            return Ok(stock);
        }

        [HttpDelete("{StockID:Guid}", Name = "DeleteStock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> DeleteStock([FromRoute] Guid StockID)
        {
            try
            {
                var deleted = await stockService.DeleteAsync(StockID);
                if (!deleted)
                    return NotFound();
                return Ok("Stock deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
