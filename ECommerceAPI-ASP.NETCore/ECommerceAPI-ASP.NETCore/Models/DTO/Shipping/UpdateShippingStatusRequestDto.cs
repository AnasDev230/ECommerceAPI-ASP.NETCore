using System.ComponentModel.DataAnnotations;
using ECommerceAPI_ASP.NETCore.Models.Domain;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Shipping
{
    public class UpdateShippingStatusRequestDto
    {
        [Required]
        public ShippingStatus Status { get; set; }
    }
}
