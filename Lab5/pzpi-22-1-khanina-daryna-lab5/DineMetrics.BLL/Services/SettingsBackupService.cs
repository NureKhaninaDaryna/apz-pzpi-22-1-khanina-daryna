using DineMetrics.BLL.Services.Interfaces;
using DineMetrics.Core.Models;
using DineMetrics.Core.Shared;
using DineMetrics.DAL;
using DineMetrics.DAL.Repositories;

namespace DineMetrics.BLL.Services;

public class SettingsBackupService(IRepository<EaterySettingsBackup> repository, IRepository<Eatery> eateryRepository) : ISettingsBackupService
{
    public async Task<ServiceResult> BackupEaterySettings(int eateryId)
    {
        var eatery = await eateryRepository.GetByIdAsync(eateryId);
        if (eatery == null) return ServiceResult.Failure(new Error("Eatery not found"));

        var backup = new EaterySettingsBackup
        {
            EateryId = eateryId,
            OperatingHours = eatery.OperatingHours,
            MaximumCapacity = eatery.MaximumCapacity,
            TemperatureThreshold = eatery.TemperatureThreshold
        };

        await repository.CreateAsync(backup);
        
        return ServiceResult.Success;
    }

    public async Task<ServiceResult> RestoreEaterySettings(int eateryId, string backupName)
    {
        var backup = (await repository
            .GetByPredicateAsync(b => b.EateryId == eateryId && b.BackupName == backupName))
            .FirstOrDefault();

        if (backup == null) return ServiceResult.Failure(new Error("Backup not found"));

        var eatery = await eateryRepository.GetByIdAsync(eateryId);
        if (eatery == null) return ServiceResult.Failure(new Error("Eatery not found"));

        eatery.OperatingHours = backup.OperatingHours;
        eatery.MaximumCapacity = backup.MaximumCapacity;
        eatery.TemperatureThreshold = backup.TemperatureThreshold;

        await eateryRepository.UpdateAsync(eatery);
        
        return ServiceResult.Success;
    }

    public async Task<List<EaterySettingsBackup>> GetBackups(int eateryId)
    {
        var backups = await repository.GetByPredicateAsync(b => b.EateryId == eateryId);

        return backups.OrderByDescending(b => b.BackupTime).ToList();
    }
}