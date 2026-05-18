using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Payment;
using ECommerceAPI_ASP.NETCore.Models.DTO.Transaction;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IOrderRepository orderRepository;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            this.paymentRepository = paymentRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<PaymentDto> CreateAsync(CreatePaymentRequestDto request)
        {
            var order = await orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");

            var existingPayment = await paymentRepository.GetByOrderIdAsync(request.OrderId);
            if (existingPayment != null)
                throw new InvalidOperationException("A payment already exists for this order.");

            var payment = new Payment
            {
                OrderId = request.OrderId,
                Method = request.Method,
                Amount = order.TotalAmount,
                Status = PaymentStatus.Pending,
            };

            await paymentRepository.CreateAsync(payment);
            return MapToDto(payment);
        }

        public async Task<PaymentDto?> GetByIdAsync(Guid id)
        {
            var payment = await paymentRepository.GetByIdAsync(id);
            if (payment == null)
                return null;

            return MapToDto(payment);
        }

        public async Task<PaymentDto?> GetByOrderIdAsync(Guid orderId)
        {
            var payment = await paymentRepository.GetByOrderIdAsync(orderId);
            if (payment == null)
                return null;

            return MapToDto(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var payments = await paymentRepository.GetAllAsync();
            return payments.Select(MapToDto);
        }

        public async Task<IEnumerable<PaymentDto>> GetByStatusAsync(string status)
        {
            if (!Enum.TryParse<PaymentStatus>(status, true, out var paymentStatus))
                throw new ArgumentException($"Invalid payment status: {status}");

            var payments = await paymentRepository.GetByStatusAsync(paymentStatus);
            return payments.Select(MapToDto);
        }

        public async Task<PaymentDto?> UpdateStatusAsync(Guid paymentId, UpdatePaymentStatusRequestDto request)
        {
            var payment = await paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
                return null;

            if (payment.Status is PaymentStatus.Completed or PaymentStatus.Refunded or PaymentStatus.Cancelled)
                throw new InvalidOperationException("Payment status cannot be updated from its current state.");

            var updated = await paymentRepository.UpdateStatusAsync(paymentId, request.Status);
            if (!updated)
                return null;

            var updatedPayment = await paymentRepository.GetByIdAsync(paymentId);
            return updatedPayment != null ? MapToDto(updatedPayment) : null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var payment = await paymentRepository.GetByIdAsync(id);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {id} not found.");

            if (payment.Status is PaymentStatus.Completed or PaymentStatus.Processing)
                throw new InvalidOperationException("Cannot delete a payment that is completed or processing.");

            return await paymentRepository.DeleteAsync(id);
        }

        public async Task<TransactionDto> AddTransactionAsync(Guid paymentId, string gatewayTransactionId, string type, decimal amount, string? gatewayRawResponse = null)
        {
            var payment = await paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");

            if (!Enum.TryParse<TransactionType>(type, true, out var transactionType))
                throw new ArgumentException($"Invalid transaction type: {type}");

            var transaction = new Transaction
            {
                PaymentId = paymentId,
                GatewayTransactionId = gatewayTransactionId,
                Amount = amount,
                Type = transactionType,
                Status = TransactionStatus.Pending,
                GatewayRawResponse = gatewayRawResponse,
            };

            await paymentRepository.CreateTransactionAsync(transaction);
            return MapTransactionToDto(transaction);
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByPaymentIdAsync(Guid paymentId)
        {
            var payment = await paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");

            var transactions = await paymentRepository.GetTransactionsByPaymentIdAsync(paymentId);
            return transactions.Select(MapTransactionToDto);
        }

        private static PaymentDto MapToDto(Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Method = payment.Method.ToString(),
                Amount = payment.Amount,
                Status = payment.Status.ToString(),
                ProcessedAt = payment.ProcessedAt,
                FailureReason = payment.FailureReason,
                CreatedAt = payment.CreatedAt,
            };
        }

        private static TransactionDto MapTransactionToDto(Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                PaymentId = transaction.PaymentId,
                GatewayTransactionId = transaction.GatewayTransactionId,
                Amount = transaction.Amount,
                Type = transaction.Type.ToString(),
                Status = transaction.Status.ToString(),
                GatewayRawResponse = transaction.GatewayRawResponse,
                ErrorCode = transaction.ErrorCode,
                ErrorDescription = transaction.ErrorDescription,
                CreatedAt = transaction.CreatedAt,
            };
        }
    }
}
