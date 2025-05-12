namespace DineMetrics.Core.Dto;

public class AnomalyDetectionDto
{
    public List<DateTime> AnomalyTimestamps { get; set; } = new();
    public double Threshold { get; set; }
    public double StandardDeviation { get; set; }
}