using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment?> GetByIdAsync(Guid id);
        Task<Payment?> GetByOrderIdAsync(Guid orderId);
        Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<bool> UpdateStatusAsync(Guid paymentId, PaymentStatus newStatus);
        Task<bool> UpdateAsync(Payment payment);
        Task<bool> DeleteAsync(Guid id);
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionsByPaymentIdAsync(Guid paymentId);
    }
}
