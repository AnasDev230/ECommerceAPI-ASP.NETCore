using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class Image : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string FileExtension { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(500)]
        public string? AltText { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Url { get; set; } = string.Empty;

        public bool IsPrimary { get; set; } = false;

        public int DisplayOrder { get; set; } = 0;
    }
}
