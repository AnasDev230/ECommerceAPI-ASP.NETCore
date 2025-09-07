using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI_ASP.NETCore.Models.DTO.Category
{
    public class CreateCategoryRequestDto
    {
        [Required]
        public string Name { get; set; }
        public Guid? ImageID { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}
