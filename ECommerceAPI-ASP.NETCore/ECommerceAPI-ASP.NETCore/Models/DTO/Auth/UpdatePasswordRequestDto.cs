using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Auth
{
    public class UpdatePasswordRequestDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
