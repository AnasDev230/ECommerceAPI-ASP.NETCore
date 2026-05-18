using ECommerceAPI_ASP.NETCore.Models.DTO.AuditLog;

namespace ECommerceAPI_ASP.NETCore.Services.Interface
{
    public interface IAuditLogService
    {
        Task<AuditLogDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<AuditLogDto>> GetAllAsync();
        Task<IEnumerable<AuditLogDto>> GetByEntityAsync(string entityType, Guid? entityId = null);
        Task<IEnumerable<AuditLogDto>> GetByUserIdAsync(string userId);
        Task<IEnumerable<AuditLogDto>> GetByActionAsync(string action);
        Task<IEnumerable<AuditLogDto>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task<bool> DeleteAsync(Guid id);
    }
}
