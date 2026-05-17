using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI_ASP.NETCore.Models.Domain
{
    public class AuditLog : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string EntityType { get; set; } = string.Empty;

        public Guid? EntityId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Action { get; set; } = string.Empty;

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? IpAddress { get; set; }

        [MaxLength(200)]
        public string? UserAgent { get; set; }
    }
}