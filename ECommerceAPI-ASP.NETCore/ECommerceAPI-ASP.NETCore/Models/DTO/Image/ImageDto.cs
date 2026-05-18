namespace ECommerceAPI_ASP.NETCore.Models.DTO
{
    public class ImageDto
    {
        public Guid ID { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string Url { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
    }
}
