namespace DineMetrics.Core.Models;

public class EaterySettingsBackup : BaseEntity
{
    public int EateryId { get; set; }
    public string BackupName { get; set; } = $"Backup_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
    public DateTime BackupTime { get; set; } = DateTime.UtcNow;

    public string OperatingHours { get; set; } = null!;
    public int MaximumCapacity { get; set; }
    public double TemperatureThreshold { get; set; }

    public virtual Eatery Eatery { get; set; } = null!;
}