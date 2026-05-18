using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.DTO.AuditLog;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository auditLogRepository;
        private readonly IMapper mapper;

        public AuditLogService(IAuditLogRepository auditLogRepository, IMapper mapper)
        {
            this.auditLogRepository = auditLogRepository;
            this.mapper = mapper;
        }

        public async Task<AuditLogDto?> GetByIdAsync(Guid id)
        {
            var auditLog = await auditLogRepository.GetByIdAsync(id);
            if (auditLog == null)
                return null;

            return mapper.Map<AuditLogDto>(auditLog);
        }

        public async Task<IEnumerable<AuditLogDto>> GetAllAsync()
        {
            var auditLogs = await auditLogRepository.GetAllAsync();
            return mapper.Map<IEnumerable<AuditLogDto>>(auditLogs);
        }

        public async Task<IEnumerable<AuditLogDto>> GetByEntityAsync(string entityType, Guid? entityId = null)
        {
            var auditLogs = await auditLogRepository.GetByEntityAsync(entityType, entityId);
            return mapper.Map<IEnumerable<AuditLogDto>>(auditLogs);
        }

        public async Task<IEnumerable<AuditLogDto>> GetByUserIdAsync(string userId)
        {
            var auditLogs = await auditLogRepository.GetByUserIdAsync(userId);
            return mapper.Map<IEnumerable<AuditLogDto>>(auditLogs);
        }

        public async Task<IEnumerable<AuditLogDto>> GetByActionAsync(string action)
        {
            var auditLogs = await auditLogRepository.GetByActionAsync(action);
            return mapper.Map<IEnumerable<AuditLogDto>>(auditLogs);
        }

        public async Task<IEnumerable<AuditLogDto>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            var auditLogs = await auditLogRepository.GetByDateRangeAsync(start, end);
            return mapper.Map<IEnumerable<AuditLogDto>>(auditLogs);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await auditLogRepository.DeleteAsync(id);
        }
    }
}
