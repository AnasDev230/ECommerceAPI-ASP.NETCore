using ECommerceAPI_ASP.NETCore.Models.DTO.AuditLog;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository auditLogRepository;

        public AuditLogService(IAuditLogRepository auditLogRepository)
        {
            this.auditLogRepository = auditLogRepository;
        }

        public async Task<AuditLogDto?> GetByIdAsync(Guid id)
        {
            var auditLog = await auditLogRepository.GetByIdAsync(id);
            if (auditLog == null)
                return null;

            return MapToDto(auditLog);
        }

        public async Task<IEnumerable<AuditLogDto>> GetAllAsync()
        {
            var auditLogs = await auditLogRepository.GetAllAsync();
            return auditLogs.Select(MapToDto);
        }

        public async Task<IEnumerable<AuditLogDto>> GetByEntityAsync(string entityType, Guid? entityId = null)
        {
            var auditLogs = await auditLogRepository.GetByEntityAsync(entityType, entityId);
            return auditLogs.Select(MapToDto);
        }

        public async Task<IEnumerable<AuditLogDto>> GetByUserIdAsync(string userId)
        {
            var auditLogs = await auditLogRepository.GetByUserIdAsync(userId);
            return auditLogs.Select(MapToDto);
        }

        public async Task<IEnumerable<AuditLogDto>> GetByActionAsync(string action)
        {
            var auditLogs = await auditLogRepository.GetByActionAsync(action);
            return auditLogs.Select(MapToDto);
        }

        public async Task<IEnumerable<AuditLogDto>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            var auditLogs = await auditLogRepository.GetByDateRangeAsync(start, end);
            return auditLogs.Select(MapToDto);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await auditLogRepository.DeleteAsync(id);
        }

        private static AuditLogDto MapToDto(ECommerceAPI_ASP.NETCore.Models.Domain.AuditLog auditLog)
        {
            return new AuditLogDto
            {
                Id = auditLog.Id,
                EntityType = auditLog.EntityType,
                EntityId = auditLog.EntityId,
                Action = auditLog.Action,
                OldValues = auditLog.OldValues,
                NewValues = auditLog.NewValues,
                UserId = auditLog.UserId,
                Description = auditLog.Description,
                IpAddress = auditLog.IpAddress,
                UserAgent = auditLog.UserAgent,
                CreatedAt = auditLog.CreatedAt,
            };
        }
    }
}
