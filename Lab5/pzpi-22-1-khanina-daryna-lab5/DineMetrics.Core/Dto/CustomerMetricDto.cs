namespace DineMetrics.Core.Dto;

public class CustomerMetricDto
{
    public int Count { get; set; } 

    public DateTime Time { get; set; }
    
    public int DeviceId { get; set; }
    
    public string DeviceModel { get; set; } = null!;
}