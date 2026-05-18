using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Stock;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class StockService : IStockService
    {
        private readonly IStockRepository stockRepository;
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public StockService(IStockRepository stockRepository, IProductRepository productRepository, IMapper mapper)
        {
            this.stockRepository = stockRepository;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<StockDto> CreateAsync(CreateStockRequestDto request)
        {
            var product = await productRepository.GetByID(request.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");

            var stock = new Stock
            {
                ProductId = request.ProductId,
                SKU = $"{product.Name.Substring(0, Math.Min(3, product.Name.Length))}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}",
                ImageID = request.ImageID,
                Color = request.Color,
                Size = request.Size,
                Quantity = request.Quantity,
                Price = request.Price,
                LowStockThreshold = 10,
                ReservedQuantity = 0,
            };

            await stockRepository.CreateAsync(stock);
            return mapper.Map<StockDto>(stock);
        }

        public async Task<IEnumerable<StockDto>> GetAllAsync()
        {
            var stocks = await stockRepository.GetAllStocksAsync();
            return mapper.Map<IEnumerable<StockDto>>(stocks);
        }

        public async Task<IEnumerable<StockDto>> GetByProductIdAsync(Guid productId)
        {
            var product = await productRepository.GetByID(productId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            var stocks = await stockRepository.GetAllByProductIdAsync(productId);
            return mapper.Map<IEnumerable<StockDto>>(stocks);
        }

        public async Task<StockDto?> GetByIdAsync(Guid id)
        {
            var stock = await stockRepository.GetByID(id);
            if (stock == null)
                return null;

            return mapper.Map<StockDto>(stock);
        }

        public async Task<StockDto?> UpdateAsync(Guid id, UpdateStockRequestDto request)
        {
            var stock = await stockRepository.GetByID(id);
            if (stock == null)
                return null;

            stock.Color = request.Color;
            stock.Size = request.Size;
            stock.Quantity = request.Quantity;

            var updated = await stockRepository.UpdateAsync(stock);
            if (!updated)
                return null;

            var updatedStock = await stockRepository.GetByID(id);
            return mapper.Map<StockDto>(updatedStock);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var stock = await stockRepository.GetByID(id);
            if (stock == null)
                throw new KeyNotFoundException($"Stock with ID {id} not found.");

            return await stockRepository.DeleteAsync(id);
        }
    }
}
