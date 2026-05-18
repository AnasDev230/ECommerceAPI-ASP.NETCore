using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        [MinLength(5)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}
