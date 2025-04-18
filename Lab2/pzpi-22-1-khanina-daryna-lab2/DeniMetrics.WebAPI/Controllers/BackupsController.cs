using DeniMetrics.WebAPI.Attributes;
using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DeniMetrics.WebAPI.Controllers;

[Authorize]
[PermissionAuthorize(ManagementName.BackupsManagement, PermissionAccess.Full)]
public class BackupsController(ISettingsBackupService settingsBackupService) : BaseController
{
    // GET: api/backups/eatery/5
    [HttpGet("eatery/{eateryId}")]
    public async Task<IActionResult> GetBackups(int eateryId)
    {
        var backups = await settingsBackupService.GetBackups(eateryId);
        return Ok(backups);
    }

    // POST: api/backups/eatery/5
    [HttpPost("eatery/{eateryId}/create")]
    public async Task<IActionResult> BackupEaterySettings(int eateryId)
    {
        var result = await settingsBackupService.BackupEaterySettings(eateryId);
        return result.IsSuccess ? Ok("Backup created successfully.") : BadRequest(result.Error);
    }

    // POST: api/backups/eatery/5/restore?backupName=Backup_20250413_103000
    [HttpPost("eatery/{eateryId}/restore")]
    public async Task<IActionResult> RestoreEaterySettings(int eateryId, [FromQuery] string backupName)
    {
        var result = await settingsBackupService.RestoreEaterySettings(eateryId, backupName);
        return result.IsSuccess ? Ok("Backup restored successfully.") : BadRequest(result.Error);
    }
}