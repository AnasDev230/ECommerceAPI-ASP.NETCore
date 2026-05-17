using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EcommerceDBContext dbContext;

        public OrderRepository(EcommerceDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Order> CreateOrderFromCartAsync(string customerId)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var cart = await dbContext.ShoppingCarts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Stock)
                    .ThenInclude(s => s.Product)
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId);

                if (cart == null || !cart.Items.Any())
                    throw new InvalidOperationException("Cart is empty.");

                var order = new Order
                {
                    CustomerId = customerId,
                    CreatedAt = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    Items = new List<OrderItem>()
                };

                foreach (var cartItem in cart.Items)
                {
                    if (cartItem.Stock == null)
                        throw new InvalidOperationException($"Stock not found for cart item.");
                    if (cartItem.Stock.Quantity < cartItem.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for {cartItem.Stock.Product?.Name}.");

                    var orderItem = new OrderItem
                    {
                        StockId = cartItem.StockId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.Stock.Price,
                    };
                    orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;
                    order.Items.Add(orderItem);
                    order.TotalAmount += orderItem.TotalPrice;

                    cartItem.Stock.Quantity -= cartItem.Quantity;
                }

                await dbContext.Orders.AddAsync(order);
                dbContext.ShoppingCartItems.RemoveRange(cart.Items);
                dbContext.ShoppingCarts.Remove(cart);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var order = await dbContext.Orders
                    .Include(o => o.Items)
                    .ThenInclude(oi => oi.Stock)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                    return false;

                foreach (var item in order.Items)
                {
                    if (item.Stock != null)
                        item.Stock.Quantity += item.Quantity;
                }

                dbContext.Orders.Remove(order);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await dbContext.Orders
                .AsNoTracking()
                .AsSplitQuery()
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Stock)
                .Include(o => o.Payment)
                .Include(o => o.Shipping)
                .Include(o => o.BillingAddress)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await dbContext.Orders
                .AsNoTracking()
                .AsSplitQuery()
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Stock)
                .ThenInclude(s => s.Product)
                .Include(o => o.Payment)
                .Include(o => o.Shipping)
                .Include(o => o.BillingAddress)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId)
        {
            return await dbContext.Orders
                .AsNoTracking()
                .AsSplitQuery()
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Stock)
                .Include(o => o.Payment)
                .Include(o => o.Shipping)
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            var order = await dbContext.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            if (order.Status is OrderStatus.Shipped or OrderStatus.Delivered or OrderStatus.Cancelled)
                return false;

            var rowsAffected = await dbContext.Orders
                .Where(o => o.Id == orderId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(o => o.Status, newStatus)
                    .SetProperty(o => o.UpdatedAt, DateTime.UtcNow)
                    .SetProperty(o => o.CompletedAt, newStatus == OrderStatus.Delivered ? DateTime.UtcNow : order.CompletedAt));

            return rowsAffected > 0;
        }
    }
}
