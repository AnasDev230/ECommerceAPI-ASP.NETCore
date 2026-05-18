using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Product
{
    public class CreateProductRequestDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(8000)]
        public string? DescriptionPlainText { get; set; }

        [MaxLength(100)]
        public string? Brand { get; set; }

        public bool IsActive { get; set; } = true;

        [Range(0, 999999.999)]
        public decimal? Weight { get; set; }

        public Guid? ImageID { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}
