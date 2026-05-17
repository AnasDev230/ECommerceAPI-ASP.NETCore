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
        Task<Payment?> UpdateStatusAsync(Guid paymentId, PaymentStatus newStatus);
        Task<Payment?> UpdateAsync(Payment payment);
        Task<Payment?> DeleteAsync(Guid id);
    }
}
