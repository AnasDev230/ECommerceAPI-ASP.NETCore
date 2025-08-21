using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Category
{
    public class CreateCategoryRequestDto
    {
        [Required]
        public string Name { get; set; }
        public string UrlHandle { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}
