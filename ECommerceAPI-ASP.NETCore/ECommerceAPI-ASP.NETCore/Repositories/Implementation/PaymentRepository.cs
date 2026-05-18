using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly EcommerceDBContext dbContext;

        public PaymentRepository(EcommerceDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            await dbContext.Payments.AddAsync(payment);
            await dbContext.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var rowsAffected = await dbContext.Payments
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await dbContext.Payments
                .AsNoTracking()
                .Include(p => p.Order)
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await dbContext.Payments
                .AsNoTracking()
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment?> GetByOrderIdAsync(Guid orderId)
        {
            return await dbContext.Payments
                .AsNoTracking()
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status)
        {
            return await dbContext.Payments
                .AsNoTracking()
                .Include(p => p.Order)
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Payment payment)
        {
            var rowsAffected = await dbContext.Payments
                .Where(p => p.Id == payment.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.Method, payment.Method)
                    .SetProperty(p => p.Amount, payment.Amount)
                    .SetProperty(p => p.Status, payment.Status)
                    .SetProperty(p => p.ProcessedAt, payment.ProcessedAt)
                    .SetProperty(p => p.FailureReason, payment.FailureReason)
                    .SetProperty(p => p.UpdatedAt, DateTime.UtcNow));

            return rowsAffected > 0;
        }

        public async Task<bool> UpdateStatusAsync(Guid paymentId, PaymentStatus newStatus)
        {
            if (newStatus == PaymentStatus.Completed)
            {
                var rowsAffected = await dbContext.Payments
                    .Where(p => p.Id == paymentId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(p => p.Status, newStatus)
                        .SetProperty(p => p.UpdatedAt, DateTime.UtcNow)
                        .SetProperty(p => p.ProcessedAt, DateTime.UtcNow));

                return rowsAffected > 0;
            }

            var rows = await dbContext.Payments
                .Where(p => p.Id == paymentId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.Status, newStatus)
                    .SetProperty(p => p.UpdatedAt, DateTime.UtcNow));

            return rows > 0;
        }
    }
}
