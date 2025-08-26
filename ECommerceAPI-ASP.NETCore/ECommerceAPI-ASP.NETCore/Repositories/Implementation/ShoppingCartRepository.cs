using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly EcommerceDBContext dBContext;

        public ShoppingCartRepository(EcommerceDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<ShoppingCart> CreateAsync(ShoppingCart shoppingCart)
        {
            await dBContext.ShoppingCarts.AddAsync(shoppingCart);
            await dBContext.SaveChangesAsync();
            return shoppingCart;
        }

        public async Task<ShoppingCart?> DeleteAsync(Guid id)
        {
           var shoppingCart=await dBContext.ShoppingCarts.FirstOrDefaultAsync(s=>s.Id==id);
            if (shoppingCart==null)
                return null;
            dBContext.ShoppingCarts.Remove(shoppingCart);
            await dBContext.SaveChangesAsync();
            return shoppingCart;
        }

        public async Task<ShoppingCart?> GetCartByCustomerIdAsync(string customerId)
        {
            return await dBContext.ShoppingCarts.FirstOrDefaultAsync(x=>x.CustomerId==customerId);
        }

        public async Task<ShoppingCart?> GetCartByID(Guid id)
        {
            return await dBContext.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
