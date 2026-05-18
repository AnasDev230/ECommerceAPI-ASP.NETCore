using ECommerceAPI_ASP.NETCore.Models.DTO.Payment;
using ECommerceAPI_ASP.NETCore.Models.DTO.Transaction;

namespace ECommerceAPI_ASP.NETCore.Services.Interface
{
    public interface IPaymentService
    {
        Task<PaymentDto> CreateAsync(CreatePaymentRequestDto request);
        Task<PaymentDto?> GetByIdAsync(Guid id);
        Task<PaymentDto?> GetByOrderIdAsync(Guid orderId);
        Task<IEnumerable<PaymentDto>> GetAllAsync();
        Task<IEnumerable<PaymentDto>> GetByStatusAsync(string status);
        Task<PaymentDto?> UpdateStatusAsync(Guid paymentId, UpdatePaymentStatusRequestDto request);
        Task<bool> DeleteAsync(Guid id);
        Task<TransactionDto> AddTransactionAsync(Guid paymentId, string gatewayTransactionId, string type, decimal amount, string? gatewayRawResponse = null);
        Task<IEnumerable<TransactionDto>> GetTransactionsByPaymentIdAsync(Guid paymentId);
    }
}
