using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EcommerceDBContext dBContext;
        public OrderRepository(EcommerceDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Order> CreateOrderAsync(string customerID)
        {
            using var transaction = await dBContext.Database.BeginTransactionAsync();
            var cart = await dBContext.ShoppingCarts.Include(c=>c.Items).ThenInclude(i=>i.Stock).ThenInclude(s => s.Product).FirstOrDefaultAsync(c=>c.CustomerId==customerID);
            if (cart == null || !cart.Items.Any())
                throw new Exception("Cart is empty.");
            var order = new Order
            {
                CustomerId = customerID,
                CreatedAt = DateTime.Now,
                Status="Pending",
                Items=new List<OrderItem>()
            };

            foreach (var cartItem in cart.Items)
            {
                if(cartItem.Stock.Quantity<cartItem.Quantity)
                    throw new Exception($"Insufficient stock for {cartItem.Stock.Product.Name}");

                var orderItem = new OrderItem
                {
                    OrderId=order.Id,
                    StockId=cartItem.StockId,
                    Quantity=cartItem.Quantity,
                    UnitPrice=cartItem.Stock.Price,
                };
                orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;
                order.Items.Add(orderItem);
                order.TotalAmount += orderItem.UnitPrice * orderItem.Quantity;
                cartItem.Stock.Quantity-=orderItem.Quantity;
            }
            await dBContext.Orders.AddAsync(order);
            dBContext.ShoppingCartItems.RemoveRange(cart.Items);
            dBContext.ShoppingCarts.Remove(cart);
            await dBContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return order;

        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            using var transaction = await dBContext.Database.BeginTransactionAsync();
            var order = await dBContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(x=>x.Id==id);
            if (order == null)
                return false;

            foreach (var item in order.Items)
            {
                item.Stock.Quantity += item.Quantity;
            }
            dBContext.Orders.Remove(order);
            dBContext.OrderItems.RemoveRange(order.Items);
            
            await dBContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await dBContext.Orders.Include(o => o.Items).ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await dBContext.Orders.Include(o => o.Items).ThenInclude(oi => oi.Stock).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId)
        {
            return await dBContext.Orders.Where(o=>o.CustomerId==customerId).ToListAsync();
        }

        public async Task<Order> UpdateOrderStatusAsync(Order order)
        {
            var existingOrder = await dBContext.Orders.FindAsync(order.Id);
            if (existingOrder.Status is "Shipped" or "Completed" or "Cancelled")
                return null;
            if (existingOrder != null)
            {
                dBContext.Entry(existingOrder).CurrentValues.SetValues(order);
                await dBContext.SaveChangesAsync();
                return order;
            }
            return null;   
        }
    }
}
