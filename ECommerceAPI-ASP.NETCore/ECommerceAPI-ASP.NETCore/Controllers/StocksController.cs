using AutoMapper;
using Azure.Core;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Stock;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockRepository stockRepository;
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;

        public StocksController(IStockRepository stockRepository,IMapper mapper,IProductRepository productRepository)
        {
            this.stockRepository = stockRepository;
            this.mapper = mapper;
            this.productRepository = productRepository;
        }
        [HttpPost("Add", Name = "AddStock")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddStock([FromBody] CreateStockRequestDto request)
        {
            var product=await productRepository.GetByID(request.ProductId);
            if (product==null)
                return NotFound("Product Not Found!!");
            
            var stock = mapper.Map<Stock>(request);
            await stockRepository.CreateAsync(stock);
            return Created("",mapper.Map<StockDto>(stock));
        }

        [HttpGet("{productID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStocksByProductID([FromRoute] Guid productID)
        {
            var product = await productRepository.GetByID(productID);
            if (product == null)
                return NotFound("Product Not Found!!");
            var stock=await stockRepository.GetAllByProductIdAsync(productID);
            return Ok(mapper.Map<List<StockDto>>(stock));

        }
        [HttpGet("GetByID/{StockID}", Name = "GetStockByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID([FromRoute] Guid StockID)
        {
            var stock=await stockRepository.GetByID(StockID);
            if (stock == null)
                return NotFound();
            return Ok(mapper.Map<StockDto>(stock));
        }
        [HttpPut("{StockID:Guid}", Name = "EditStock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStock([FromRoute] Guid StockID,[FromBody] UpdateStockRequestDto request)
        {
            var stock = await stockRepository.GetByID(StockID);
            if (stock == null)
                return NotFound();
            stock=mapper.Map(request,stock);
            await stockRepository.UpdateAsync(stock);
            return Ok(mapper.Map<StockDto>(stock));
        }
        [HttpDelete]
        [Route("{StockID:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid StockID)
        {
            var stock = await stockRepository.GetByID(StockID);
            if (stock == null)
                return NotFound();
            stock = await stockRepository.DeleteAsync(StockID);
            return Ok(mapper.Map<StockDto>(stock));
        }
    }
}
