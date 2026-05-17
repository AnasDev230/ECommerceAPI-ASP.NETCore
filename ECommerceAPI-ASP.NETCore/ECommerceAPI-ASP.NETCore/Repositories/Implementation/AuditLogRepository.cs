using ECommerceAPI_ASP.NETCore.Data;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Repositories.Implementation
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly EcommerceDBContext dbContext;

        public AuditLogRepository(EcommerceDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<AuditLog> CreateAsync(AuditLog auditLog)
        {
            await dbContext.AuditLogs.AddAsync(auditLog);
            await dbContext.SaveChangesAsync();
            return auditLog;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var rowsAffected = await dbContext.AuditLogs
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await dbContext.AuditLogs
                .AsNoTracking()
                .Include(a => a.User)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByActionAsync(string action)
        {
            return await dbContext.AuditLogs
                .AsNoTracking()
                .Include(a => a.User)
                .Where(a => a.Action == action)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await dbContext.AuditLogs
                .AsNoTracking()
                .Include(a => a.User)
                .Where(a => a.CreatedAt >= start && a.CreatedAt <= end)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, Guid? entityId = null)
        {
            var query = dbContext.AuditLogs
                .AsNoTracking()
                .Include(a => a.User)
                .Where(a => a.EntityType == entityType);

            if (entityId.HasValue)
            {
                query = query.Where(a => a.EntityId == entityId.Value);
            }

            return await query
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId)
        {
            return await dbContext.AuditLogs
                .AsNoTracking()
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<AuditLog?> GetByIdAsync(Guid id)
        {
            return await dbContext.AuditLogs
                .AsNoTracking()
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
