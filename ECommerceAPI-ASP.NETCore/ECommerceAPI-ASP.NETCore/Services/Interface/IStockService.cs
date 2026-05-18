using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Stock;

namespace ECommerceAPI_ASP.NETCore.Services.Interface
{
    public interface IStockService
    {
        Task<StockDto> CreateAsync(CreateStockRequestDto request);
        Task<IEnumerable<StockDto>> GetAllAsync();
        Task<IEnumerable<StockDto>> GetByProductIdAsync(Guid productId);
        Task<StockDto?> GetByIdAsync(Guid id);
        Task<StockDto?> UpdateAsync(Guid id, UpdateStockRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
