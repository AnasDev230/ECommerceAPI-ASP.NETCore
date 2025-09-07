namespace ECommerceAPI_ASP.NETCore.Models.DTO.Product
{
    public class CreateProductRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ImageID { get; set; }
        public Guid CategoryId { get; set; }
    }
}
