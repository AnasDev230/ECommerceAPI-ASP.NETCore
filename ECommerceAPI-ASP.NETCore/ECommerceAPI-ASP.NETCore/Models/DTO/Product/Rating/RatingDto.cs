namespace ECommerceAPI_ASP.NETCore.Models.DTO.Product.Rating
{
    public class RatingDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public string CustomerId { get; set; }
        public int Stars { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
