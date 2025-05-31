namespace DineMetrics.Core.Dto;

public class ReportDto
{
    public double AverageTemperature { get; set; }

    public int TotalCustomers { get; set; }

    public DateOnly ReportDate { get; set; }
}