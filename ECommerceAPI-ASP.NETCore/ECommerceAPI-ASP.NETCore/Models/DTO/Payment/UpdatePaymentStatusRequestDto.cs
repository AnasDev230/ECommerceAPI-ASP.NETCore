using System.ComponentModel.DataAnnotations;
using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Payment
{
    public class UpdatePaymentStatusRequestDto
    {
        [Required]
        public PaymentStatus Status { get; set; }
    }
}
