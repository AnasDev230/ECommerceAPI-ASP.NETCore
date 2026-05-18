namespace ECommerceAPI_ASP.NETCore.Models.DTO.Transaction
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public string GatewayTransactionId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? GatewayRawResponse { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorDescription { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
