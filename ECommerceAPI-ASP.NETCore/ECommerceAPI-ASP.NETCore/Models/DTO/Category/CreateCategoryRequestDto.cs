using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Category
{
    public class CreateCategoryRequestDto
    {
        [Required]
        [MinLength(10)]
        public string Name { get; set; }
        public string UrlHandle { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}
