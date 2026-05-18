using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class Transaction : BaseEntity
    {
        [Required]
        public Guid PaymentId { get; set; }
        public Payment? Payment { get; set; }

        [Required]
        [MaxLength(100)]
        public string GatewayTransactionId { get; set; } = string.Empty;

        [Range(0, 999999.99)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public TransactionType Type { get; set; } = TransactionType.Capture;

        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

        public string? GatewayRawResponse { get; set; }

        [MaxLength(100)]
        public string? ErrorCode { get; set; }

        [MaxLength(500)]
        public string? ErrorDescription { get; set; }
    }

    public enum TransactionType
    {
        Authorize = 0,    
        Capture = 1,     
        Refund = 2,       
        Void = 3,        
        Chargeback = 4
    }

    public enum TransactionStatus
    {
        Pending = 0,
        Success = 1,
        Failed = 2
    }
}
