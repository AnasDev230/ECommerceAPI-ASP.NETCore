using ECommerceAPI_ASP.NETCore.Models.DTO.AuditLog;
using ECommerceAPI_ASP.NETCore.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI_ASP.NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            this.auditLogService = auditLogService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAuditLogs()
        {
            var auditLogs = await auditLogService.GetAllAsync();
            return Ok(auditLogs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAuditLogById([FromRoute] Guid id)
        {
            var auditLog = await auditLogService.GetByIdAsync(id);
            if (auditLog == null)
                return NotFound();
            return Ok(auditLog);
        }

        [HttpGet("Entity/{entityType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAuditLogsByEntity([FromRoute] string entityType, [FromQuery] Guid? entityId = null)
        {
            var auditLogs = await auditLogService.GetByEntityAsync(entityType, entityId);
            return Ok(auditLogs);
        }

        [HttpGet("User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAuditLogsByUserId([FromQuery] string userId)
        {
            var auditLogs = await auditLogService.GetByUserIdAsync(userId);
            return Ok(auditLogs);
        }

        [HttpGet("Action/{action}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAuditLogsByAction([FromRoute] string action)
        {
            var auditLogs = await auditLogService.GetByActionAsync(action);
            return Ok(auditLogs);
        }

        [HttpGet("DateRange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAuditLogsByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            if (start > end)
                return BadRequest("Start date must be before end date.");

            var auditLogs = await auditLogService.GetByDateRangeAsync(start, end);
            return Ok(auditLogs);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuditLog([FromRoute] Guid id)
        {
            var deleted = await auditLogService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return Ok("Audit log deleted successfully");
        }
    }
}
