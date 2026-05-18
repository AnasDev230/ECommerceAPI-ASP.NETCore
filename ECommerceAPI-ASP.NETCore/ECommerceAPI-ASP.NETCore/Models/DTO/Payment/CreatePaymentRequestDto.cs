using System.ComponentModel.DataAnnotations;
using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Payment
{
    public class CreatePaymentRequestDto
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }
    }
}
