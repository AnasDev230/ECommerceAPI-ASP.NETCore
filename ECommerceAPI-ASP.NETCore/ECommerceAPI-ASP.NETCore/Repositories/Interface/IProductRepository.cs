using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<IEnumerable<Stock>> GetAllStockAsync();
        public Task<IEnumerable<Product>> GetAllProductsByCategoryID(Guid ID);
        Task<Product?> GetByID(Guid id);
        Task<Product?> UpdateAsync(Product product);
        Task<Product?> DeleteAsync(Guid id);
    }
}
