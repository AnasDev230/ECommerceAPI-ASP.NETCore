using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Product.Rating
{
    public class UpdateRatingRequestDto
    {
        [Range(1, 5, ErrorMessage = "Stars must be between 1 and 5.")]
        public int Stars { get; set; }
        public string? Comment { get; set; }
    }
}
