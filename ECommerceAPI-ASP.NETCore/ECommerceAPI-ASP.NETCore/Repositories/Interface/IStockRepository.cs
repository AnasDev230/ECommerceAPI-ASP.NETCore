using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface IStockRepository
    {
        Task<Stock> CreateAsync(Stock stock);
        Task<IEnumerable<Stock>> GetAllByProductIdAsync(Guid id);
        Task<Stock?> GetByID(Guid id);
        Task<Stock?> UpdateAsync(Stock stock);
        Task<Stock?> DeleteAsync(Guid id);
    }
}
