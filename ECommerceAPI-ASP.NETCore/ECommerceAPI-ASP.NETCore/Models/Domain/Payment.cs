using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class Payment : BaseEntity
    {
        [Required]
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public PaymentMethod Method { get; set; } = PaymentMethod.CreditCard;

        [Range(0, 999999.99)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public DateTime? ProcessedAt { get; set; }

        [MaxLength(500)]
        public string? FailureReason { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    public enum PaymentMethod
    {
        CreditCard = 0,
        DebitCard = 1,
        PayPal = 2,
        BankTransfer = 3,
        CashOnDelivery = 4,
        ApplePay = 5,
        GooglePay = 6
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Processing = 1,
        Completed = 2,
        Failed = 3,
        Refunded = 4,
        Cancelled = 5
    }
}