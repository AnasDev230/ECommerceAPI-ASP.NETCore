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

        public async Task<Payment?> DeleteAsync(Guid id)
        {
            var payment = await dbContext.Payments.FindAsync(id);
            if (payment == null)
                return null;

            dbContext.Payments.Remove(payment);
            await dbContext.SaveChangesAsync();
            return payment;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await dbContext.Payments
                .Include(p => p.Order)
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await dbContext.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment?> GetByOrderIdAsync(Guid orderId)
        {
            return await dbContext.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status)
        {
            return await dbContext.Payments
                .Include(p => p.Order)
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<Payment?> UpdateAsync(Payment payment)
        {
            var existingPayment = await dbContext.Payments.FirstOrDefaultAsync(p => p.Id == payment.Id);
            if (existingPayment == null)
                return null;

            existingPayment.Method = payment.Method;
            existingPayment.Amount = payment.Amount;
            existingPayment.TransactionId = payment.TransactionId;
            existingPayment.TransactionDetails = payment.TransactionDetails;
            existingPayment.Status = payment.Status;
            existingPayment.ProcessedAt = payment.ProcessedAt;
            existingPayment.FailureReason = payment.FailureReason;
            existingPayment.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return existingPayment;
        }

        public async Task<Payment?> UpdateStatusAsync(Guid paymentId, PaymentStatus newStatus)
        {
            var existingPayment = await dbContext.Payments.FindAsync(paymentId);
            if (existingPayment == null)
                return null;

            existingPayment.Status = newStatus;
            existingPayment.UpdatedAt = DateTime.UtcNow;

            if (newStatus == PaymentStatus.Completed)
                existingPayment.ProcessedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return existingPayment;
        }
    }
}
