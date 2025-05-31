using DineMetrics.Core.Models;
using DineMetrics.Core.Shared;

namespace DineMetrics.BLL.Services.Interfaces;

public interface ISettingsBackupService
{
    Task<ServiceResult> BackupEaterySettings(int eateryId);
    Task<ServiceResult> RestoreEaterySettings(int eateryId, string backupName);
    Task<List<EaterySettingsBackup>> GetBackups(int eateryId);
}